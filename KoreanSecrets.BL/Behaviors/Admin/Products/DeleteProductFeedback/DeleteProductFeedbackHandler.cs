using KoreanSecrets.Domain.Common.Constants;
using KoreanSecrets.Domain.DbConnection;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KoreanSecrets.BL.Behaviors.Admin.Products.DeleteProductFeedback;

public class DeleteProductFeedbackHandler : IRequestHandler<DeleteProductFeedbackCommand>
{
    private readonly DataContext _context;

    public DeleteProductFeedbackHandler(DataContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteProductFeedbackCommand request, CancellationToken cancellationToken)
    {
        var volume = await _context.Feedbacks
            .FirstOrDefaultAsync(t => t.Id == request.FeedbackId, cancellationToken);

        if (volume is null) throw new Exception(ErrorMessages.ProductNotFound("Коментар не знайдено!"));

        _context.Feedbacks.Remove(volume);
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
