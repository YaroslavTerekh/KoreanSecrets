using KoreanSecrets.Domain.Common.Constants;
using KoreanSecrets.Domain.Common.CustomExceptions;
using KoreanSecrets.Domain.Common.Enums;
using KoreanSecrets.Domain.DbConnection;
using KoreanSecrets.Domain.Entities;
using KoreanSecrets.Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KoreanSecrets.BL.Services;
using Newtonsoft.Json;

namespace KoreanSecrets.BL.Behaviors.Products.CheckPromocode;

public class CheckPromocodeHandler : IRequestHandler<CheckPromocodeCommand, PromocodeUI?>
{
    private readonly DataContext _context;

    public CheckPromocodeHandler(DataContext context)
    {
        _context = context;
    }

    public async Task<PromocodeUI?> Handle(CheckPromocodeCommand request, CancellationToken cancellationToken)
    {
        var promocode = await _context.Promocodes
            .FirstOrDefaultAsync(t => t.Code == request.Promocode, cancellationToken);

        if (promocode is null)
            throw new NotFoundException(ErrorMessages.PromoNotFound);

        if (promocode.StartDate > DateTime.UtcNow)
            throw new Exception(ErrorMessages.PromocodeHasBeenNotStartedYet(promocode.StartDate.ToString("dd.MM.yyyy")));

        if (promocode.EndDate < DateTime.UtcNow)
            throw new Exception(ErrorMessages.PromocodeIsExpired(promocode.EndDate.ToString("dd.MM.yyyy")));

        return new PromocodeUI
        {
            Id = promocode.Id,
            Discount = promocode.Discount,
            Title = promocode.Code,
            Total = await GetNewPriceAsync(request.CurrentUserId, promocode, cancellationToken)
        };
    }

    private async Task<decimal> GetNewPriceAsync(Guid userId, Promocode promocode, CancellationToken cancellationToken)
    {
        var user = await _context.Users
            .Include(t => t.AddressInfo)
            .Include(t => t.Bucket)
                .ThenInclude(t => t.BucketProducts)
                    .ThenInclude(t => t.Product)
            .Include(t => t.Bucket)
                .ThenInclude(t => t.BucketProducts)
                    .ThenInclude(t => t.Volume)
            .FirstOrDefaultAsync(t => t.Id == userId, cancellationToken);

        if (user is null)
            throw new NotFoundException(ErrorMessages.UserNotFound);

        if (user.Bucket.BucketProducts.Count < 1)
            throw new Exception(ErrorMessages.BucketIsEmpty);

        var bucketProducts = user.Bucket.BucketProducts
            .Select(t => new PurchasedProduct
            {
                ProductIdentify = t.ProductId.ToString(),
                Product = JsonConvert.SerializeObject(t.Product, Formatting.None, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }),
                VolumeIdentify = t.VolumeId.ToString(),
                Volume = JsonConvert.SerializeObject(t.Volume, Formatting.None, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }),
                Amount = t.Amount,
                CreatedDate = t.CreatedDate,
                ProductTitle = t.Product.Title,
            }).ToList();

        var purchasesPriceDictionary = new Dictionary<PurchasedProduct, decimal>();

        foreach (var purchaseProduct in bucketProducts)
        {
            var ppVolume = JsonConvert.DeserializeObject<Volume>(purchaseProduct.Volume);

            purchasesPriceDictionary.Add(purchaseProduct, purchaseProduct.Amount * ppVolume.Price);
        }
        
        var products = bucketProducts.Select(t => JsonConvert.DeserializeObject<Product>(t.Product)).ToList();
        var productBrandIds = products.Select(t => t.BrandId).ToList();
        var promotions = await _context.Promotions.Where(t => productBrandIds.Contains(t.BrandId)).ToListAsync(cancellationToken);

        foreach (var purchasePricePair in purchasesPriceDictionary)
        {
            purchasesPriceDictionary[purchasePricePair.Key] = CalculatePriceService.GetProductPrice(
                purchasePricePair,
                promotions,
                promocode);
        }
        
        return purchasesPriceDictionary.Select(t => t.Value).Sum();
    }
}
