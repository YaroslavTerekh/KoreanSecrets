using KoreanSecrets.Domain.Common.Constants;
using KoreanSecrets.Domain.Common.CustomExceptions;
using KoreanSecrets.Domain.DbConnection;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KoreanSecrets.BL.Behaviors.Admin.Promocodes.DeletePromocodeProduct;

public class DeletePromocodeProductHandler : IRequestHandler<DeletePromocodeProductCommand>
{
    private readonly DataContext _context;

    public DeletePromocodeProductHandler(DataContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeletePromocodeProductCommand request, CancellationToken cancellationToken)
    {
        var promocode = await _context
            .ProductPromocode.FirstOrDefaultAsync(t => 
                t.PromocodeId == request.PromocodeId 
                && t.ProductId == request.ProductId, cancellationToken);

        if (promocode is null)
            throw new NotFoundException(ErrorMessages.PromoNotFound);

        _context.ProductPromocode.Remove(promocode);
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
