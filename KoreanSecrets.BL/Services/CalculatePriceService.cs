using KoreanSecrets.Domain.Common.Enums;
using KoreanSecrets.Domain.DataTransferObjects;
using KoreanSecrets.Domain.Entities;

namespace KoreanSecrets.BL.Services;

public static class CalculatePriceService
{
    public static void GetProductPrice(
        BucketProductDTO product,
        List<Promotion>? promotions,
        Promocode? promocode)
    {
        var time = DateTime.UtcNow.Date.AddHours(-11);
        
        decimal price = 0;
            
        if (product.Product.DiscountPrice != null
            && product.Product.DiscountPrice.HasValue
            && product.Product.UseDiscountPrice)
        {
            price = product.Volume.Price -  ((product.Volume.Price * product.Product.DiscountPrice.Value) / 100);
        }
        else if (promotions != null
                 && promotions.Any(x => x.BrandId == product.Product.BrandId)
                 && product.Product.Icon == ProductIcon.Sale)
        {
            var currentPromotion = promotions.FirstOrDefault(x => 
                x is { EndDate: not null, StartDate: not null }
                 && x.BrandId == product.Product.BrandId
                 && x.StartDate.Value.Date >= time);

            if (currentPromotion is not null)
            {
                price = product.Volume.Price - ((product.Volume.Price * currentPromotion.Discount) / 100);
            }
        } 
        else if (promocode != null)
        {
            if (promocode.BrandId.HasValue)
            {
                if (product.Product.BrandId == promocode.BrandId
                    && promocode.Products.All(x => x.ProductId != product.Id)
                    && promocode.StartDate.Date >= time)
                {
                    price = product.Volume.Price -  ((product.Volume.Price  * promocode.Discount) / 100);
                } 
            }
            else
            {
                if (promocode.Products.All(x => x.ProductId != product.Id)
                    && promocode.StartDate.Date >= time)
                {
                    price = product.Volume.Price - ((product.Volume.Price  * promocode.Discount) / 100);
                } 
            }
        }

        if (price > 0)
        {
            product.Volume.PriceWithDiscount = Math.Round(price);
        }
    }
    
    public static void GetProductPrice(
        ListProductDTO product,
        List<Promotion>? promotions,
        Promocode? promocode)
    {
        var time = DateTime.UtcNow.Date.AddHours(-11);
        
        foreach (var volume in product.Volumes)
        {
            decimal price = 0;
            
            if (product.DiscountPrice != null
                && product.DiscountPrice.HasValue
                && product.UseDiscountPrice)
            {
                price = volume.Price -  ((volume.Price * product.DiscountPrice.Value) / 100);
            }
            else if (promotions != null
                     && promotions.Any(x => x.BrandId == product.BrandId)
                     && product.Icon == ProductIcon.Sale)
            {
                
                var currentPromotion = promotions.FirstOrDefault(x => 
                    x is { EndDate: not null, StartDate: not null }
                     && x.BrandId == product.BrandId
                     && x.StartDate.Value.Date >= time);

                if (currentPromotion is not null)
                {
                    price = volume.Price -  ((volume.Price * currentPromotion.Discount) / 100);
                }
            } 
            else if (promocode != null)
            {
                if (promocode.BrandId.HasValue)
                {
                    if (product.BrandId == promocode.BrandId
                        && promocode.Products.All(x => x.ProductId != product.Id)
                        && promocode.StartDate.Date >= time)
                    {
                        price = volume.Price -  ((volume.Price  * promocode.Discount) / 100);
                    } 
                }
                else
                {
                    if (promocode.Products.All(x => x.ProductId != product.Id)
                        && promocode.StartDate.Date >= time)
                    {
                        price = volume.Price -  ((volume.Price  * promocode.Discount) / 100);
                    } 
                }
            }

            if (price > 0)
            {
                volume.PriceWithDiscount = Math.Round(price);
            }
        }
    }
    
    public static void GetProductPrice(
        Product product,
        List<Promotion>? promotions,
        Promocode? promocode)
    {
        var time = DateTime.UtcNow.Date.AddHours(-11);
        
        foreach (var volume in product.Volumes)
        {
            decimal price = 0;
            
            if (product.DiscountPrice != null
                && product.DiscountPrice.HasValue
                && product.UseDiscountPrice)
            {
                price = volume.Price -  ((volume.Price * product.DiscountPrice.Value) / 100);
            }
            else if (promotions != null
                     && promotions.Any(x => x.BrandId == product.BrandId)
                     && product.AdditionalIcon == ProductIcon.Sale)
            {
                var currentPromotion = promotions.FirstOrDefault(x => 
                    x is { EndDate: not null, StartDate: not null }
                     && x.BrandId == product.BrandId
                     && x.StartDate.Value.Date >= time);

                if (currentPromotion is not null)
                {
                    price = volume.Price -  ((volume.Price * currentPromotion.Discount) / 100);
                }
            } 
            else if (promocode != null)
            {
                if (promocode.BrandId.HasValue)
                {
                    if (product.BrandId == promocode.BrandId
                        && promocode.Products.All(x => x.ProductId != product.Id)
                        && promocode.StartDate.Date >= time)
                    {
                        price = volume.Price -  ((volume.Price  * promocode.Discount) / 100);
                    } 
                }
                else
                {
                    if (promocode.Products.All(x => x.ProductId != product.Id)
                        && promocode.StartDate.Date >= time)
                    {
                        price = volume.Price -  ((volume.Price  * promocode.Discount) / 100);
                    } 
                }
            }

            if (price > 0)
            {
                volume.PriceWithDiscount = Math.Round(price);
            }
        }
    }
    
    public static decimal GetProductPrice( 
        KeyValuePair<PurchasedProduct, decimal> purchasePricePair,
        List<Promotion>? promotions,
        Promocode? promocode
        )
    {
        var time = DateTime.UtcNow.Date.AddHours(-11);
        
        var purchaseProduct = purchasePricePair.Key;
        var currentPrice = purchasePricePair.Value;

        var result = currentPrice;

        if (purchaseProduct.Product.DiscountPrice != null
            && purchaseProduct.Product.DiscountPrice.HasValue
            && purchaseProduct.Product.UseDiscountPrice)
        {
            result = currentPrice - ((currentPrice * purchaseProduct.Product.DiscountPrice.Value) / 100);
        }
        else if (promotions != null
                 && promotions.Any(x => x.BrandId == purchaseProduct.Product.BrandId)
                 && purchaseProduct.Product.AdditionalIcon == ProductIcon.Sale)
        {
            var currentPromotion = promotions.FirstOrDefault(x => 
                x is { EndDate: not null, StartDate: not null }
                && x.BrandId == purchaseProduct.Product.BrandId
                && x.StartDate.Value.Date >= time);

            if (currentPromotion is not null)
            {
                result = currentPrice - ((currentPrice * currentPromotion.Discount) / 100);
            }
        } 
        else if (promocode != null)
        {
            if (promocode.BrandId.HasValue)
            {
                if (purchaseProduct.Product.BrandId == promocode.BrandId
                    && promocode.Products.All(x => x.ProductId != purchaseProduct.Product.Id)
                    && promocode.StartDate.Date >= time)
                {
                    result = currentPrice - ((currentPrice * promocode.Discount) / 100);
                } 
            }
            else
            {
                if (promocode.Products.All(x => x.ProductId != purchaseProduct.Product.Id)
                    && promocode.StartDate.Date >= time)
                {
                    result = currentPrice - ((currentPrice * promocode.Discount) / 100);
                } 
            }
        }

        return Math.Round(result);
    }
}