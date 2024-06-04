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

        var products = await _context.Products.Where(t => t.BrandId == promotion.BrandId).ToListAsync(cancellationToken);

        foreach (var product in products)
        {
            product.AdditionalIcon = ProductIcon.Sale;
        }

        await _context.Promotions.AddAsync(promotion, cancellationToken);
        await _context.SaveChangesAsync();

        return Unit.Value;
    }
}
