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

    private async Task<long> GetNewPriceAsync(Guid userId, Promocode promocode, CancellationToken cancellationToken)
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
                ProductId = t.ProductId,
                Product = t.Product,
                VolumeId = t.VolumeId,
                Volume = t.Volume,
                Amount = t.Amount,
                CreatedDate = t.CreatedDate,
            }).ToList();

        var purchasesPriceDictionary = new Dictionary<PurchasedProduct, long>();

        foreach (var purchaseProduct in bucketProducts)
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

        var productBrandIds = bucketProducts.Select(t => t.Product.BrandId).ToList();
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

                    if (newPrice != null)
                        purchasesPriceDictionary[purchasePricePair.Key] = (long)newPrice;
                }
            }
        }

        return purchasesPriceDictionary.Select(t => t.Value).Sum();
    }
}
