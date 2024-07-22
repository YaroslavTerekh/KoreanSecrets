using Hangfire;
using KoreanSecrets.BL.Services;
using KoreanSecrets.Domain.Common.Constants;
using KoreanSecrets.Domain.Common.CustomExceptions;
using KoreanSecrets.Domain.Common.Enums;
using KoreanSecrets.Domain.DbConnection;
using KoreanSecrets.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoreanSecrets.BL.Behaviors.Admin.Users.AddPurchaseToUser;

public class AddPurchaseToUserHandler : IRequestHandler<AddPurchaseToUserCommand, long>
{
    private readonly DataContext _context;

    public AddPurchaseToUserHandler(DataContext context)
    {
        _context = context;
    }

    public async Task<long> Handle(AddPurchaseToUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _context.Users
            .Include(t => t.AddressInfo)
            .Include(t => t.AdminBucket)
                .ThenInclude(t => t.BucketProducts)
                    .ThenInclude(t => t.Product)
            .Include(t => t.AdminBucket)
                .ThenInclude(t => t.BucketProducts)
                    .ThenInclude(t => t.Volume)
            .FirstOrDefaultAsync(t => t.Id == request.CurrentUserId, cancellationToken);

        if (user is null)
            throw new NotFoundException(ErrorMessages.UserNotFound);

        if (user.AddressInfo is null && request.Address is null)
            throw new NotFoundException(ErrorMessages.AddressInfoNotFound);

        if (user.AdminBucket.BucketProducts.Count < 1)
            throw new Exception(ErrorMessages.BucketIsEmpty);

        Promocode? promocode = null;

        if (!string.IsNullOrEmpty(request.Promocode))
        {
            promocode = await _context.Promocodes.
                Include(x => x.Products)
                .FirstOrDefaultAsync(t => t.Code == request.Promocode, cancellationToken);

            if (promocode is null && request.Promocode != "")
                throw new NotFoundException(ErrorMessages.PromoNotFound);
        }

        var productIds = user.AdminBucket.BucketProducts.Select(t => t.Id).ToList();

        var client =
            await _context.Users.FirstOrDefaultAsync(x => x.PhoneNumber.Contains(request.Phone), cancellationToken);
        
        var purchase = new Purchase
        {
            Warehouse = request.Address.Warehouse,
            City = request.Address.City,
            UserId = client?.Id ?? user.Id,
            PurchaseStatus = PurchaseStatus.New,
            GeneratedBy = PurchaseGenerateBy.GeneratedByAdmin,
            AdminNotes = request.Comment,
            PayType = PayType.Terminal,
            PromocodeId = promocode is null ? null : promocode.Id,
            UserInfo = request.UserInfo,
            Phone = request.Phone,
            Email = request?.Email ?? null,
            Comment = string.Empty
        };

        purchase.Products = await _context.BucketProducts
            .Include(t => t.Product)
                .ThenInclude(t => t.MainPhoto)
            .Include(t => t.Product)
                .ThenInclude(t => t.Brand)
            .Include(t => t.Volume)
                .ThenInclude(t => t.Photos)
            .Where(t => productIds.Contains(t.Id))
            .Select(t => new PurchasedProduct
            {
                ProductIdentify = t.ProductId.ToString(),
                Product = JsonConvert.SerializeObject(t.Product, Formatting.None, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }),
                VolumeIdentify = t.VolumeId.ToString(),
                Volume = JsonConvert.SerializeObject(t.Volume, Formatting.None, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }),
                Amount = t.Amount,
                CreatedDate = t.CreatedDate,
                PurchaseId = purchase.Id,
                ProductTitle = t.Product.Title,
            }).ToListAsync(cancellationToken: cancellationToken);

        if (purchase.Products.Count < 1)
        {
            throw new NotFoundException(ErrorMessages.ProductNotFound("Продуктів для покупки"));
        }

        purchase.PurchaseIdentifier = ConvertGuidToLong(purchase.Id);

        var purchasesPriceDictionary = new Dictionary<PurchasedProduct, decimal>();

        foreach (var purchaseProduct in purchase.Products)
        {
            var volume = JsonConvert.DeserializeObject<Volume>(purchaseProduct.Volume);

            purchasesPriceDictionary.Add(purchaseProduct, purchaseProduct.Amount * volume.Price);
        }

        var products = purchase.Products.Select(t => JsonConvert.DeserializeObject<Product>(t.Product)).ToList();
        var productBrandIds = products.Select(t => t.BrandId).ToList();
        var promotions = await _context.Promotions.Where(t => productBrandIds.Contains(t.BrandId)).ToListAsync(cancellationToken);

        foreach (var purchasePricePair in purchasesPriceDictionary)
        {
            purchasesPriceDictionary[purchasePricePair.Key] = CalculatePriceService.GetProductPrice(
                purchasePricePair,
                promotions,
                promocode);
        }

        purchase.TotalPrice = purchasesPriceDictionary.Select(t => t.Value).Sum();

        if (request.Address is not null && request.SaveAddress)
        {
            if (user.AddressInfoId is not null)
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


        var bucketProducts = await _context.BucketProducts.Where(t => t.AdminBucketId == user.AdminBucket.Id).ToListAsync(cancellationToken);

        _context.BucketProducts.RemoveRange(bucketProducts);
        await _context.SaveChangesAsync(cancellationToken);

        return purchase.PurchaseIdentifier;
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
}
