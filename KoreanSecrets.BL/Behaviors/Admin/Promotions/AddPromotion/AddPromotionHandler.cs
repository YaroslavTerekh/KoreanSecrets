using Hangfire;
using KoreanSecrets.Domain.Common.Enums;
using KoreanSecrets.Domain.DbConnection;
using KoreanSecrets.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoreanSecrets.BL.Behaviors.Admin.Promotions.AddPromotion;

public class AddPromotionHandler : IRequestHandler<AddPromotionCommand>
{
    private readonly DataContext _context;

    public AddPromotionHandler(DataContext context)
    {
        _context = context;
    }
    public async Task<Unit> Handle(AddPromotionCommand request, CancellationToken cancellationToken)
    {
        var promotion = new Promotion
        {
            BrandId = request.BrandId,
            Discount = request.Discount,
            EndDate = request.EndDate.Value.AddHours(12),
            StartDate = request.StartDate.Value.AddHours(12),
        };

        await _context.Promotions.AddAsync(promotion, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        
        if (promotion.StartDate.Value.Date.ToUniversalTime() <= DateTime.UtcNow.Date)
        {
            await SetIconAsync(promotion.Id, promotion.EndDate.Value, promotion.StartDate.Value);
        }
        else
        {
            BackgroundJob.Schedule(() => SetIconAsync(promotion.Id, promotion.EndDate.Value, promotion.StartDate.Value), promotion.StartDate.Value.Date.ToUniversalTime());
        }
        
        if(request.EndDate != null && request.StartDate != null)
            BackgroundJob.Schedule(() => RemoveDiscountAsync(promotion.Id, promotion.EndDate.Value, promotion.StartDate.Value), promotion.EndDate.Value.Date.ToUniversalTime());

        return Unit.Value;
    }
    
    public async Task SetIconAsync(Guid id, DateTime endDate, DateTime startDate)
    {
        var promotion = await _context.Promotions.FirstOrDefaultAsync(t => t.Id == id);

        if (promotion is null)
            return;

        if (endDate != promotion.EndDate)
            return;

        if (startDate != promotion.StartDate)
            return;

        var products = await _context.Products.Where(t => t.BrandId == promotion.BrandId).ToListAsync();

        foreach (var product in products)
        {
            product.AdditionalIcon = ProductIcon.Sale;
        }

        _context.Products.UpdateRange(products);
        await _context.SaveChangesAsync();
    }

    public async Task RemoveDiscountAsync(Guid id, DateTime endDate, DateTime startDate)
    {
        var promotion = await _context.Promotions.FirstOrDefaultAsync(t => t.Id == id);

        if (promotion is null)
            return;

        if (endDate != promotion.EndDate)
            return;

        if (startDate != promotion.StartDate)
            return;

        _context.Promotions.Remove(promotion);
        await _context.SaveChangesAsync();
    }
}
