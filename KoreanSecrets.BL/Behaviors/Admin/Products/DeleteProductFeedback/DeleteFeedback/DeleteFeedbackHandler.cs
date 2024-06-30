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
        var reply = await _context.FeedbackReply.FirstOrDefaultAsync(t => t.Id == request.Id, cancellationToken);

        if (reply is null)
            throw new NotFoundException(ErrorMessages.CommentNotFound);
        
        _context.FeedbackReply.Remove(reply);

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
