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

namespace KoreanSecrets.BL.Behaviors.Products.TogglePromocodeStatus;

public class TogglePromocodeStatusHandler : IRequestHandler<TogglePromocodeStatusCommand>
{
    private readonly DataContext _context;

    public TogglePromocodeStatusHandler(DataContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(TogglePromocodeStatusCommand request, CancellationToken cancellationToken)
    {
        var promocode = await _context.Promocodes.FirstOrDefaultAsync(t => t.Id == request.PromocodeId, cancellationToken);

        if (promocode is null)
            throw new NotFoundException(ErrorMessages.PromoNotFound);

        promocode.IsActive = !promocode.IsActive;

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
