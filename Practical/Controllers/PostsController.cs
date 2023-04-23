using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Practical.Models;

namespace Practical.Controllers
{
    public class PostsController : Controller
    {
        private readonly StackOverflow2010Context _context;
        private const int PageSize = 10; // Number of records to display per page


        private readonly IMemoryCache _memoryCache;
        private readonly IHubContext<NotificationHub> _notificationHubContext;


        public PostsController(StackOverflow2010Context context, IMemoryCache memoryCache, IHubContext<NotificationHub> notificationHubContext)
        {
            _context = context;
            _memoryCache = memoryCache;
            _notificationHubContext = notificationHubContext;

        }

public async Task<IActionResult> Index(int page = 1, string phrase = null)
{
    if (_context.Posts == null)
    {
        return Problem("Entity set 'StackOverflow2010Context.Posts' is null.");
    }

    var posts = _context.Posts
        .AsNoTracking()
        .Where(p => p.PostTypeId == 1 || p.PostTypeId == 2);

    if (!string.IsNullOrWhiteSpace(phrase))
    {
        posts = posts.Where(p => StackOverflow2010Context.FreeText(p.Title, phrase) || StackOverflow2010Context.FreeText(p.Body, phrase));
    }

    // Calculate the total number of pages using the cached total post count
    var totalCount = await posts.CountAsync();
    var pageCount = (int)Math.Ceiling(totalCount / (double)PageSize);

    // Fetch the records for the current page
    var viewPosts = await posts
        .Skip((page - 1) * PageSize)
        .Take(PageSize)
        .Select(post => new PostUpdated(post)
        {
            User = _context.Users.FirstOrDefault(u => u.Id == post.OwnerUserId),
            TotalVotes = _context.Votes.Where(v => v.PostId == post.Id).Sum(v => v.VoteTypeId),
            UserBadges = _context.Badges.Where(b => b.UserId == post.OwnerUserId).Select(b => b.Name).ToList()
        })
        .ToListAsync();

    // Create the ViewModel
    var viewModel = new PostListViewModel
    {
        UpdatedPosts = viewPosts,
        TotalPages = pageCount,
        CurrentPage = page
    };

    return View(viewModel);
}




        // GET: Posts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Posts == null)
            {
                return NotFound();
            }

            var post = await _context.Posts
                .FirstOrDefaultAsync(m => m.Id == id);
            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        // GET: Posts/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Posts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,AcceptedAnswerId,AnswerCount,Body,ClosedDate,CommentCount,CommunityOwnedDate,CreationDate,FavoriteCount,LastActivityDate,LastEditDate,LastEditorDisplayName,LastEditorUserId,OwnerUserId,ParentId,PostTypeId,Score,Tags,Title,ViewCount")] Post post)
        {
            if (ModelState.IsValid)
            {
                _context.Add(post);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(post);
        }

        // GET: Posts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Posts == null)
            {
                return NotFound();
            }

            var post = await _context.Posts.FindAsync(id);
            if (post == null)
            {
                return NotFound();
            }
            return View(post);
        }

        // POST: Posts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,AcceptedAnswerId,AnswerCount,Body,ClosedDate,CommentCount,CommunityOwnedDate,CreationDate,FavoriteCount,LastActivityDate,LastEditDate,LastEditorDisplayName,LastEditorUserId,OwnerUserId,ParentId,PostTypeId,Score,Tags,Title,ViewCount")] Post post)
        {
            if (id != post.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(post);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PostExists(post.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                await _notificationHubContext.Clients.All.SendAsync("ReceiveUpdateNotification", $"Post {id} has been updated.");
                return RedirectToAction(nameof(Index));
            }
            return View(post);
        }

        // GET: Posts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Posts == null)
            {
                return NotFound();
            }

            var post = await _context.Posts
                .FirstOrDefaultAsync(m => m.Id == id);
            if (post == null)
            {
                return NotFound();
            }
            return View(post);
        }
        // POST: Posts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Posts == null)
            {
                return Problem("Entity set 'StackOverflow2010Context.Posts'  is null.");
            }
            var post = await _context.Posts.FindAsync(id);
            if (post != null)
            {
                _context.Posts.Remove(post);
            }

            await _context.SaveChangesAsync();
            await _notificationHubContext.Clients.All.SendAsync("ReceiveDeleteNotification", $"Post {id} has been deleted.");
            return RedirectToAction(nameof(Index));
        }


        private bool PostExists(int id)
        {
          return (_context.Posts?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
