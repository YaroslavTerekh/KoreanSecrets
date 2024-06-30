using KoreanSecrets.Domain.Common.Constants;
using KoreanSecrets.Domain.Common.CustomExceptions;
using KoreanSecrets.Domain.DbConnection;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoreanSecrets.BL.Behaviors.Admin.Users.DeleteFeedback;

public class DeleteFeedbackHandler : IRequestHandler<DeleteFeedbackCommand>
{
    private readonly DataContext _context;

    public DeleteFeedbackHandler(DataContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteFeedbackCommand request, CancellationToken cancellationToken)
    {
        var feedback = await _context.Feedbacks.FirstOrDefaultAsync(t => t.Id == request.Id, cancellationToken);
        var reply = await _context.FeedbackReply.FirstOrDefaultAsync(t => t.Id == request.Id, cancellationToken);

        if (feedback is null && reply is null)
            throw new NotFoundException(ErrorMessages.CommentNotFound);

        if (feedback is not null)
            _context.Feedbacks.Remove(feedback);

        if (reply is not null)
            _context.FeedbackReply.Remove(reply);

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
