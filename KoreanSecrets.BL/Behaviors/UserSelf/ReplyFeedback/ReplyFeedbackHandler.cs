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

namespace KoreanSecrets.BL.Behaviors.UserSelf.ReplyFeedback;

public class ReplyFeedbackHandler : IRequestHandler<ReplyFeedbackCommand>
{
    private readonly DataContext _context;

    public ReplyFeedbackHandler(DataContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(ReplyFeedbackCommand request, CancellationToken cancellationToken)
    {
        if (!await _context.Feedbacks.AnyAsync(t => t.Id == request.FeedbackId))
            throw new NotFoundException(ErrorMessages.CommentNotFound);

        if(!await _context.Users.AnyAsync(t => t.Id == request.UserId))
            throw new NotFoundException(ErrorMessages.UserNotFound);

        var feedbackReply = new FeedbackReply
        {
            FeedbackId = request.FeedbackId,
            UserId = request.UserId,
            ReplyText = request.ReplyText,
        };

        await _context.FeedbackReply.AddAsync(feedbackReply, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
