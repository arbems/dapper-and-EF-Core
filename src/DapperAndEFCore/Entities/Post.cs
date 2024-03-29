﻿using DapperAndEFCore.Entities.Common;

namespace DapperAndEFCore.Entities;

public class Post : Entity
{
    public int UserId { get; set; }
    public string Title { get; set; }
    public string Body { get; set; }
    public PostDetail Detail { get; set; }

    private List<Comment> _comments = new();
    public ICollection<Comment> Comments => _comments;
}
