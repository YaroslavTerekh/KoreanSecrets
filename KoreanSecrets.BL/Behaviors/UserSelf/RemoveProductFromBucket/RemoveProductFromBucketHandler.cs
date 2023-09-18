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

namespace KoreanSecrets.BL.Behaviors.UserSelf.RemoveProductFromBucket;

public class RemoveProductFromBucketHandler : IRequestHandler<RemoveProductFromBucketCommand>
{
    private readonly DataContext _context;

    public RemoveProductFromBucketHandler(DataContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(RemoveProductFromBucketCommand request, CancellationToken cancellationToken)
    {
        var product = await _context.Products
            .FirstOrDefaultAsync(t => t.Id == request.ProductId, cancellationToken);

        if (product is null)
            throw new NotFoundException(ErrorMessages.SomeProductNotFound);

        var user = await _context.Users
            .Include(t => t.Bucket)
                .ThenInclude(t => t.PurchaseProducts)
            .FirstOrDefaultAsync(t => t.Id == request.CurrentUserId, cancellationToken);

        if (user is null)
            throw new NotFoundException(ErrorMessages.UserNotFound);

        var purchaseProduct = user.Bucket.PurchaseProducts.Where(t => t.ProductId == request.ProductId).FirstOrDefault();

        if (purchaseProduct is null)
            throw new NotFoundException(ErrorMessages.SomeProductNotFound);

        _context.PurchasedProducts.Remove(purchaseProduct);
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
