using Hangfire;
using KoreanSecrets.BL.Services.Abstractions;
using KoreanSecrets.Domain.Common.Constants;
using KoreanSecrets.Domain.Common.CustomExceptions;
using KoreanSecrets.Domain.Common.Enums;
using KoreanSecrets.Domain.DbConnection;
using KoreanSecrets.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KoreanSecrets.BL.Behaviors.Purchases.GeneratePurchase;

public class GeneratePurchaseHandler : IRequestHandler<GeneratePurchaseCommand, string>
{
    private readonly DataContext _context;
    private readonly ILiqPayService _liqPayService;

    public GeneratePurchaseHandler(DataContext context, ILiqPayService liqPayService)
    {
        _context = context;
        _liqPayService = liqPayService;
    }

    public async Task<string> Handle(GeneratePurchaseCommand request, CancellationToken cancellationToken)
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
            PromocodeId = promocode is null ? null : promocode.Id,
            Products = user.Bucket.PurchaseProducts
        };

        purchase.PurchaseIdentifier = ConvertGuidToLong(purchase.Id);

        var totalPrice = purchase.Products.Select(t => t.Product.Price * t.Amount).Sum();

        if(promocode is not null)
            totalPrice -= (long)((totalPrice * promocode.Discount) / 100);

        purchase.TotalPrice = totalPrice;

        await _context.Purchases.AddAsync(purchase, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        BackgroundJob.Schedule(() => ModifyPurchaseDeliveryState(purchase.Id, PurchaseStatus.Success), TimeSpan.FromDays(15));
        BackgroundJob.Schedule(() => DeleteFailuredPurchase(purchase.Id), TimeSpan.FromMinutes(15));

        return await _liqPayService.GenerateForm(purchase.Id, cancellationToken);
    }

    private long ConvertGuidToLong(Guid guid)
    {
        byte[] guidBytes = guid.ToByteArray();
        byte[] longBytes = new byte[8];
        Array.Copy(guidBytes, 0, longBytes, 0, 8);

        long result = BitConverter.ToInt64(longBytes, 0);

        result = Math.Abs(result % 100000);

        return result;
    }

    public async Task ModifyPurchaseDeliveryState(Guid id, PurchaseStatus status)
    {
        var purchase = await _context.Purchases.FirstOrDefaultAsync(t => t.Id == id);

        purchase.PurchaseStatus = status;

        await _context.SaveChangesAsync();
    }

    public async Task DeleteFailuredPurchase(Guid id)
    {
        var purchase = await _context.Purchases.FirstOrDefaultAsync(t => t.Id == id);

        if (purchase.PurchaseStatus == PurchaseStatus.Waiting || purchase.PurchaseStatus == PurchaseStatus.Failure)
        { 
            _context.Purchases.Remove(purchase);
            await _context.SaveChangesAsync();
        }
    }
}