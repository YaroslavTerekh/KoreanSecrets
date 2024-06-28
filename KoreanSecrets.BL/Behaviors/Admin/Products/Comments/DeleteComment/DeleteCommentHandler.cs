using KoreanSecrets.Domain.Common.Constants;
using KoreanSecrets.Domain.Common.CustomExceptions;
using KoreanSecrets.Domain.DbConnection;
using KoreanSecrets.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoreanSecrets.BL.Behaviors.Admin.Products.Comments.DeleteComment;

public class DeleteCommentHandler : IRequestHandler<DeleteCommentCommand>
{
    private readonly DataContext _context;

    public DeleteCommentHandler(DataContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteCommentCommand request, CancellationToken cancellationToken)
    {
        using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);

        try
        {
            var comment = await _context.Comment
                .Include(c => c.Replies)
                    .ThenInclude(t => t.Replies)
                .FirstOrDefaultAsync(t => t.Id == request.CommentId, cancellationToken);

            if (comment is null)
                throw new NotFoundException(ErrorMessages.CommentNotFound);

            await DeleteRepliesAsync(comment, cancellationToken);
            _context.Comment.Remove(comment);
            await _context.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);

            return Unit.Value;
        }
        catch (Exception)
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }

    private async Task DeleteRepliesAsync(Comment comment, CancellationToken cancellationToken)
    {
        foreach (var reply in comment.Replies)
        {
            await DeleteRepliesAsync(reply, cancellationToken);
            _context.Comment.Remove(reply);
        }
    }
}
