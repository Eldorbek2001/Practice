using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.AspNetCore.Components.Web.Virtualization;
using Practical.Models;

public class PostUpdated : Post
{
    public virtual User User { get; set; }

    public virtual int TotalVotes { get; set; } = 0;
    public virtual List<string> UserBadges { get; set; } = new List<string>() { };

    public PostUpdated(Post post)
    {
        Id = post.Id;
        AcceptedAnswerId = post.AcceptedAnswerId;
        AnswerCount = post.AnswerCount;
        Body = post.Body;
        ClosedDate = post.ClosedDate;
        CommentCount = post.CommentCount;
        CommunityOwnedDate = post.CommunityOwnedDate;
        CreationDate = post.CreationDate;
        FavoriteCount = post.FavoriteCount;
        LastActivityDate = post.LastActivityDate;
        LastEditDate = post.LastEditDate;
        LastEditorDisplayName = post.LastEditorDisplayName;
        LastEditorUserId = post.LastEditorUserId;
        OwnerUserId = post.OwnerUserId;
        ParentId = post.ParentId;
        PostTypeId = post.PostTypeId;
        Score = post.Score;
        Tags = post.Tags;
        Title = post.Title;
        ViewCount = post.ViewCount;       
        User = new User();
        UserBadges = new List<string>();
    }



}

