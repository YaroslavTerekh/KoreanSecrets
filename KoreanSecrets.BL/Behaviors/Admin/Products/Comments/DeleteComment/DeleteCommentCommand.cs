using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoreanSecrets.BL.Behaviors.Admin.Products.Comments.DeleteComment;

public class DeleteCommentCommand : IRequest
{
    public Guid CommentId { get; set; }

    public DeleteCommentCommand(Guid id) => CommentId = id;
}
