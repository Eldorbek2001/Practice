using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Practical.Models;

namespace Practical.Controllers
{
    public class VoteTypesController : Controller
    {
        private readonly StackOverflow2010Context _context;

        public VoteTypesController(StackOverflow2010Context context)
        {
            _context = context;
        }

        // GET: VoteTypes
        public async Task<IActionResult> Index()
        {
              return _context.VoteTypes != null ? 
                          View(await _context.VoteTypes.ToListAsync()) :
                          Problem("Entity set 'StackOverflow2010Context.VoteTypes'  is null.");
        }

        // GET: VoteTypes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.VoteTypes == null)
            {
                return NotFound();
            }

            var voteType = await _context.VoteTypes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (voteType == null)
            {
                return NotFound();
            }

            return View(voteType);
        }

        // GET: VoteTypes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: VoteTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] VoteType voteType)
        {
            if (ModelState.IsValid)
            {
                _context.Add(voteType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(voteType);
        }

        // GET: VoteTypes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.VoteTypes == null)
            {
                return NotFound();
            }

            var voteType = await _context.VoteTypes.FindAsync(id);
            if (voteType == null)
            {
                return NotFound();
            }
            return View(voteType);
        }

        // POST: VoteTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] VoteType voteType)
        {
            if (id != voteType.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(voteType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VoteTypeExists(voteType.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(voteType);
        }

        // GET: VoteTypes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.VoteTypes == null)
            {
                return NotFound();
            }

            var voteType = await _context.VoteTypes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (voteType == null)
            {
                return NotFound();
            }

            return View(voteType);
        }

        // POST: VoteTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.VoteTypes == null)
            {
                return Problem("Entity set 'StackOverflow2010Context.VoteTypes'  is null.");
            }
            var voteType = await _context.VoteTypes.FindAsync(id);
            if (voteType != null)
            {
                _context.VoteTypes.Remove(voteType);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VoteTypeExists(int id)
        {
          return (_context.VoteTypes?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
