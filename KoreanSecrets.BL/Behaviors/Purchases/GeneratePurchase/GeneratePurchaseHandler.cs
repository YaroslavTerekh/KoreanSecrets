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

public class GeneratePurchaseHandler : IRequestHandler<GeneratePurchaseCommand, object>
{
    private readonly DataContext _context;
    private readonly ILiqPayService _liqPayService;

    public GeneratePurchaseHandler(DataContext context, ILiqPayService liqPayService)
    {
        _context = context;
        _liqPayService = liqPayService;
    }

    public async Task<object> Handle(GeneratePurchaseCommand request, CancellationToken cancellationToken)
    {
        var user = await _context.Users
            .Include(t => t.AddressInfo)
            .Include(t => t.Bucket)
                .ThenInclude(t => t.PurchaseProducts)
                    .ThenInclude(t => t.Product)
            .Include(t => t.Bucket)
                .ThenInclude(t => t.PurchaseProducts)
                    .ThenInclude(t => t.Volume)
            .FirstOrDefaultAsync(t => t.Id == request.CurrentUserId, cancellationToken);

        if (user is null)
            throw new NotFoundException(ErrorMessages.UserNotFound);

        if (user.AddressInfo is null && request.Address is null)
            throw new NotFoundException(ErrorMessages.AddressInfoNotFound);

        if (user.Bucket.PurchaseProducts.Count < 1)
            throw new Exception(ErrorMessages.BucketIsEmpty);

        var promocode = await _context.Promocodes
            .FirstOrDefaultAsync(t => t.Code == request.Promocode && t.IsActive, cancellationToken);

        if (promocode is null && request.Promocode != "")
            throw new NotFoundException(ErrorMessages.PromoNotFound);

        var purchase = new Purchase
        {
            Warehouse = request.Address.Warehouse,
            City = request.Address.City,
            UserId = user.Id,
            PurchaseStatus = PurchaseStatus.Waiting,
            Comment = request.Comment,
            PayType = request.PayType,
            PromocodeId = promocode is null ? null : promocode.Id,
            Products = user.Bucket.PurchaseProducts
        };

        purchase.PurchaseIdentifier = ConvertGuidToLong(purchase.Id);

        var totalPrice = purchase.Products.Select(t => t.Volume.Price * t.Amount).Sum();

        if(promocode is not null)
            totalPrice -= (long)((totalPrice * promocode.Discount) / 100);

        purchase.TotalPrice = totalPrice;

        if(request.Address is not null && request.SaveAddress)
        {
            if(user.AddressInfoId is not null)
            {
                var addressInfo = await _context.Addresses.FirstOrDefaultAsync(t => t.Id == user.AddressInfoId, cancellationToken);

                addressInfo.Warehouse = request.Address.Warehouse;
                addressInfo.City = request.Address.City;
            } 
            else
            {
                var addressOfUser = new AddressInfo
                {
                    City = request.Address.City,
                    Warehouse = request.Address.Warehouse,
                    UserId = user.Id
                };

                await _context.Addresses.AddAsync(addressOfUser, cancellationToken);
            }
        }

        await _context.Purchases.AddAsync(purchase, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        BackgroundJob.Schedule(() => ModifyPurchaseDeliveryState(purchase.Id, PurchaseStatus.Success), TimeSpan.FromDays(15));
        BackgroundJob.Schedule(() => DeleteFailuredPurchase(purchase.Id), TimeSpan.FromMinutes(15));
 
        var form = await _liqPayService.GenerateForm(purchase.Id, cancellationToken);

        return new { Form = form, Id = purchase.PurchaseIdentifier };
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