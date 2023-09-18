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

namespace KoreanSecrets.BL.Behaviors.UserSelf.ChangeProductAmount;

public class ChangeProductAmountHandler : IRequestHandler<ChangeProductAmountCommand>
{
    private readonly DataContext _context;

    public ChangeProductAmountHandler(DataContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(ChangeProductAmountCommand request, CancellationToken cancellationToken)
    {
        var userBucket = await _context.Users
            .Where(t => t.Id == request.CurrentUserId)
            .Include(t => t.Bucket)
                .ThenInclude(t => t.PurchaseProducts)
                    .ThenInclude(t => t.Product)
            .Select(t => t.Bucket)
            .FirstOrDefaultAsync(cancellationToken);

        if (userBucket is null)
            throw new NotFoundException(ErrorMessages.UserNotFound);

        var purchaseProduct = userBucket.PurchaseProducts.FirstOrDefault(t => t.Id == request.PurchasedProductId);

        if (purchaseProduct is null)
            throw new Exception(ErrorMessages.PurchaseProductNotRelatedToUser);

        purchaseProduct = await _context.PurchasedProducts.FirstOrDefaultAsync(t => t.Id == request.PurchasedProductId);

        purchaseProduct.Amount = request.NewAmount;

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
