using Hangfire;
using KoreanSecrets.BL.Services.Abstractions;
using KoreanSecrets.Domain.Common.Constants;
using KoreanSecrets.Domain.Common.CustomExceptions;
using KoreanSecrets.Domain.Common.Enums;
using KoreanSecrets.Domain.DbConnection;
using KoreanSecrets.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;

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
                .ThenInclude(t => t.BucketProducts)
                    .ThenInclude(t => t.Product)
            .Include(t => t.Bucket)
                .ThenInclude(t => t.BucketProducts)
                    .ThenInclude(t => t.Volume)
            .FirstOrDefaultAsync(t => t.Id == request.CurrentUserId, cancellationToken);

        if (user is null)
            throw new NotFoundException(ErrorMessages.UserNotFound);

        if (user.AddressInfo is null && request.Address is null)
            throw new NotFoundException(ErrorMessages.AddressInfoNotFound);

        if (user.Bucket.BucketProducts.Count < 1)
            throw new Exception(ErrorMessages.BucketIsEmpty);

        var promocode = await _context.Promocodes
            .FirstOrDefaultAsync(t => t.Code == request.Promocode, cancellationToken);

        if (promocode is null && request.Promocode != "")
            throw new NotFoundException(ErrorMessages.PromoNotFound);

        var productIds = user.Bucket.BucketProducts.Select(t => t.Id).ToList();
        //var productsIds = user.Bucket.PurchaseProducts.Select(t => t.ProductId).ToList();
        //var totalProductDiscount = await _context.Products.Where(t => productsIds.Contains(t.Id)).Select(t => t.)

        var purchase = new Purchase
        {
            Warehouse = request.Address.Warehouse,
            City = request.Address.City,
            UserId = user.Id,
            PurchaseStatus = PurchaseStatus.New,
            Comment = request.Comment,
            PayType = request.PayType,
            PromocodeId = promocode is null ? null : promocode.Id,
        };

        purchase.Products = await _context.BucketProducts.Where(t => productIds.Contains(t.Id))
            .Select(t => new PurchasedProduct
            {
                ProductId = t.ProductId,
                Product = t.Product,
                VolumeId = t.VolumeId,
                Volume = t.Volume,
                Amount = t.Amount,
                CreatedDate = t.CreatedDate,
                PurchaseId = purchase.Id
            }).ToListAsync();

        if (purchase.Products.Count < 1)
        {
            throw new NotFoundException(ErrorMessages.ProductNotFound("Продуктів для покупки"));
        }

        purchase.PurchaseIdentifier = ConvertGuidToLong(purchase.Id);

        var purchasesPriceDictionary = new Dictionary<PurchasedProduct, long>();

        foreach (var purchaseProduct in purchase.Products)
        {
            purchasesPriceDictionary.Add(purchaseProduct, purchaseProduct.Amount * purchaseProduct.Volume.Price);
        }

        // product discount

        foreach (var purchasePricePair in purchasesPriceDictionary)
        {
            var purchaseProduct = purchasePricePair.Key;
            var currentPrice = purchasePricePair.Value;
            long? newPrice = null;

            if (purchaseProduct.Product.DiscountPrice is not null)
            {
                newPrice = (currentPrice * purchaseProduct.Product.DiscountPrice) / 100;
            }
            
            if (newPrice != null) 
                purchasesPriceDictionary[purchasePricePair.Key] = (long)newPrice;
        }

        // promocode

        if (promocode is not null)
        {
            foreach (var purchasePricePair in purchasesPriceDictionary)
            {
                var purchaseProduct = purchasePricePair.Key;
                var currentPrice = purchasePricePair.Value;

                if (purchaseProduct.Product.BrandId == promocode.BrandId)
                {
                    purchasesPriceDictionary[purchasePricePair.Key] = currentPrice - (long)((currentPrice * promocode.Discount) / 100);
                }
            }
        }

        // promotion

        var productBrandIds = purchase.Products.Select(t => t.Product.BrandId).ToList();
        var promotions = await _context.Promotions.Where(t => productBrandIds.Contains(t.BrandId)).ToListAsync(cancellationToken);
        if (promotions.Count > 0)
        {
            var promoBrandIds = promotions.Select(t => t.BrandId).ToList();
            foreach (var purchasePricePair in purchasesPriceDictionary)
            {
                var purchaseProduct = purchasePricePair.Key;
                var currentPrice = purchasePricePair.Value;
                long? newPrice = null;

                if (purchaseProduct.Product.BrandId is not null && promoBrandIds.Contains((Guid)purchaseProduct.Product.BrandId))
                {
                    newPrice = (long?)(currentPrice * promotions.Where(t => t.BrandId == purchaseProduct.Product.BrandId).Select(t => t.Discount).FirstOrDefault()) / 100;

                    if(newPrice != null)
                        purchasesPriceDictionary[purchasePricePair.Key] = (long)newPrice;
                }
            }
        }

        purchase.TotalPrice = purchasesPriceDictionary.Select(t => t.Value).Sum();

        if (request.Address is not null && request.SaveAddress)
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

        if(request.PayType == PayType.Terminal)
        {
            return new { Id = purchase.PurchaseIdentifier };
        }

        BackgroundJob.Schedule(() => ModifyPurchaseDeliveryState(purchase.Id, PurchaseStatus.Success), TimeSpan.FromHours(24));
        BackgroundJob.Schedule(() => DeleteFailuredPurchase(purchase.Id), TimeSpan.FromHours(3));
 
        var form = await _liqPayService.GenerateForm(purchase.Id, cancellationToken);

        return new { Form = form, Id = purchase.PurchaseIdentifier };
    }

    private long? GetTotalPrice(PurchasedProduct purchasedProduct, Promocode? promocode, long? discount)
    {
        if (purchasedProduct.Product.DiscountPrice is not null && purchasedProduct.Product.BrandId == promocode?.BrandId)
        {
            return
                ((purchasedProduct.Volume.Price * purchasedProduct.Amount) - (((purchasedProduct.Volume.Price * purchasedProduct.Amount * purchasedProduct.Product.DiscountPrice)) / 100) - discount);
        }
        else if (purchasedProduct.Product.DiscountPrice is not null)
        {
            return        
                (purchasedProduct.Volume.Price * purchasedProduct.Amount) - ((purchasedProduct.Volume.Price * purchasedProduct.Amount * purchasedProduct.Product.DiscountPrice) / 100);
        }
        else
        {
            return 
                purchasedProduct.Volume.Price * purchasedProduct.Amount;
        }
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