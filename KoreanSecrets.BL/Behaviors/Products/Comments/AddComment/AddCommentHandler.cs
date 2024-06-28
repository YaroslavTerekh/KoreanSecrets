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

namespace KoreanSecrets.BL.Behaviors.Products.Comments.AddComment;

public class AddCommentHandler : IRequestHandler<AddCommentCommand>
{
    private readonly DataContext _context;

    public AddCommentHandler(DataContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(AddCommentCommand request, CancellationToken cancellationToken)
    {
        if (!await _context.Products.AnyAsync(t => t.Id == request.ProductId))
            throw new NotFoundException(ErrorMessages.SomeProductNotFound);

        if (!await _context.Users.AnyAsync(t => t.Id == request.UserId))
            throw new NotFoundException(ErrorMessages.UserNotFound);

        if (request.ParentCommentId != null && !await _context.Comment.AnyAsync(t => t.Id == request.ParentCommentId))
            throw new NotFoundException(ErrorMessages.CommentNotFound);

        var comment = new Comment
        {
            CommentText = request.CommentText,
            ParentCommentId = request.ParentCommentId,
            ProductId = request.ProductId,
            UserId = request.UserId
        };

        await _context.Comment.AddAsync(comment, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
