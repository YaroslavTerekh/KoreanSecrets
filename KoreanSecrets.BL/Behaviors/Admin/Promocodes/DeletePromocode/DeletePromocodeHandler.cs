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

namespace KoreanSecrets.BL.Behaviors.Admin.Promocodes.DeletePromocode;

public class DeletePromocodeHandler : IRequestHandler<DeletePromocodeCommand>
{
    private readonly DataContext _context;

    public DeletePromocodeHandler(DataContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeletePromocodeCommand request, CancellationToken cancellationToken)
    {
        var promocode = await _context.Promocodes.FirstOrDefaultAsync(t => t.Id == request.PromocodeId, cancellationToken);

        if (promocode is null)
            throw new NotFoundException(ErrorMessages.PromoNotFound);

        _context.Promocodes.Remove(promocode);
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
