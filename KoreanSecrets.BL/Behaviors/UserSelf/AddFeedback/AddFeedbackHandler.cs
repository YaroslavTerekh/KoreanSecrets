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

namespace KoreanSecrets.BL.Behaviors.UserSelf.AddFeedback;

public class AddFeedbackHandler : IRequestHandler<AddFeedbackCommand>
{
    private readonly DataContext _context;

    public AddFeedbackHandler(DataContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(AddFeedbackCommand request, CancellationToken cancellationToken)
    {
        var product = await _context.Products
            .Include(t => t.Feedbacks)
            .FirstOrDefaultAsync(t => t.Id == request.ProductId, cancellationToken);

        if (product is null)
            throw new NotFoundException(ErrorMessages.SomeProductNotFound);

        var feedback = new Feedback
        {
            ProductId = request.ProductId,
            FeedbackText = request.FeedbackText,
            Rate = request.Rate,
            UserId = request.CurrentUserId
        };

        await _context.Feedbacks.AddAsync(feedback, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
