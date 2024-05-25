using KoreanSecrets.Domain.Common.Constants;
using KoreanSecrets.Domain.Common.CustomExceptions;
using KoreanSecrets.Domain.DbConnection;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoreanSecrets.BL.Behaviors.Admin.Promotions.ModifyPromotion;

public class ModifyPromotionHandler : IRequestHandler<ModifyPromotionCommand>
{
    private readonly DataContext _context;

    public ModifyPromotionHandler(DataContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(ModifyPromotionCommand request, CancellationToken cancellationToken)
    {
        var promotion = await _context.Promotions.FirstOrDefaultAsync(t => t.Id == request.Id, cancellationToken);

        if(promotion is null)
            throw new NotFoundException(ErrorMessages.PromotionNotFound);

        promotion.StartDate = request.StartDate;
        promotion.EndDate = request.EndDate;
        promotion.BrandId = request.BrandId;
        promotion.Discount = request.Discount;

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
