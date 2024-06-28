using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace KoreanSecrets.BL.Behaviors.Products.Comments.AddComment;

public class AddCommentCommand : IRequest
{
    public string CommentText { get; set; }

    public Guid ProductId { get; set; }

    public Guid? ParentCommentId { get; set; }

    [JsonIgnore]
    public Guid UserId { get; set; }
}
