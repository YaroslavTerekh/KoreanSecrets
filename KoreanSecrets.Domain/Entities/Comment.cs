using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoreanSecrets.Domain.Entities;

public class Comment : BaseEntity
{
    public string CommentText { get; set; }

    public Guid UserId { get; set; }

    public User User { get; set; }

    public Guid ProductId { get; set; }

    public Product Product { get; set; }

    public Guid? ParentCommentId { get; set; }
    public Comment ParentComment { get; set; }

    public List<Comment> Replies { get; set; } = new();
}
