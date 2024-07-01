using KoreanSecrets.Domain.DataTransferObjects;
using KoreanSecrets.Domain.DbConnection;
using KoreanSecrets.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KoreanSecrets.BL.Behaviors.UserSelf.GePurchaseById;

public class GePurchaseByIdHandler : IRequestHandler<GePurchaseByIdQuery, Purchase>
{
    private readonly DataContext _context;

    public GePurchaseByIdHandler(DataContext context)
    {
        _context = context;
    }

    public async Task<Purchase> Handle(GePurchaseByIdQuery request, CancellationToken cancellationToken)
    {
        var userPurchases = await _context.Purchases
            .Where(t => t.PurchaseIdentifier == request.Id && t.UserId == request.CurrentUserId)
            .Include(x=>x.Products)
                .ThenInclude(p => p.Product)
            .Include(x=>x.Products)
                .ThenInclude(p => p.Product)
            .FirstOrDefaultAsync(cancellationToken);

        return userPurchases;
    }
}
