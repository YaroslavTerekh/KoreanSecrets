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

namespace KoreanSecrets.BL.Behaviors.Admin.Promotions.DeletePromotion;

public class DeletePromotionHandler : IRequestHandler<DeletePromotionCommand>
{
    private readonly DataContext _context;

    public DeletePromotionHandler(DataContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeletePromotionCommand request, CancellationToken cancellationToken)
    {
        var promo = await _context.Promotions.FirstOrDefaultAsync(t => t.Id == request.PromoId, cancellationToken);

        if (promo is null)
            throw new NotFoundException(ErrorMessages.PromotionNotFound);

        _context.Promotions.Remove(promo);
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
