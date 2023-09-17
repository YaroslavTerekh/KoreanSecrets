using KoreanSecrets.Domain.Common.Constants;
using KoreanSecrets.Domain.Common.CustomExceptions;
using KoreanSecrets.Domain.Common.Enums;
using KoreanSecrets.Domain.DbConnection;
using KoreanSecrets.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KoreanSecrets.BL.Behaviors.Purchases.GeneratePurchase;

public class GeneratePurchaseHandler : IRequestHandler<GeneratePurchaseCommand>
{
    private readonly DataContext _context;

    public GeneratePurchaseHandler(DataContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(GeneratePurchaseCommand request, CancellationToken cancellationToken)
    {
        var user = await _context.Users
            .Include(t => t.Bucket)
                .ThenInclude(t => t.PurchaseProducts)
                    .ThenInclude(t => t.Product)
            .FirstOrDefaultAsync(t => t.Id == request.CurrentUserId, cancellationToken);

        if (user is null)
            throw new NotFoundException(ErrorMessages.UserNotFound);

        if (user.Bucket.PurchaseProducts.Count < 1)
            throw new Exception(ErrorMessages.BucketIsEmpty);

        var promocode = await _context.Promocodes
            .FirstOrDefaultAsync(t => t.Code == request.Promocode, cancellationToken);

        if (promocode is null && request.Promocode is not null)
            throw new NotFoundException(ErrorMessages.PromoNotFound);

        var purchase = new Purchase
        {
            UserId = user.Id,
            PurchaseStatus = PurchaseStatus.Waiting,
            Comment = request.Comment,
            PayType = request.PayType,
            PromocodeId = promocode.Id,
            Products = user.Bucket.PurchaseProducts
        };

        purchase.PurchaseIdentifier = ConvertGuidToLong(purchase.Id);

        var totalPrice = purchase.Products.Select(t => t.Product.Price * t.Amount).Sum();

        if(promocode is not null)
            totalPrice -= (long)((totalPrice * promocode.Discount) / 100);

        purchase.TotalPrice = totalPrice;

        await _context.Purchases.AddAsync(purchase, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }

    private long ConvertGuidToLong(Guid guid)
    {
        byte[] guidBytes = guid.ToByteArray();
        byte[] longBytes = new byte[8];
        Array.Copy(guidBytes, 0, longBytes, 0, 8);

        long result = BitConverter.ToInt64(longBytes, 0);

        // Отримати лише перші 5 цифр
        result = Math.Abs(result % 100000);

        return result;
    }
}