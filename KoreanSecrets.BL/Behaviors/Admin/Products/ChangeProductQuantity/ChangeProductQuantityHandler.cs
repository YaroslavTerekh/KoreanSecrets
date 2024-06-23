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

namespace KoreanSecrets.BL.Behaviors.Admin.Products.ChangeProductQuantity;

public class ChangeProductQuantityHandler : IRequestHandler<ChangeProductQuantityCommand>
{
    private readonly DataContext _context;

    public ChangeProductQuantityHandler(DataContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(ChangeProductQuantityCommand request, CancellationToken cancellationToken)
    {
        var volume = await _context.Volume.FirstOrDefaultAsync(t => t.Id == request.VolumeId, cancellationToken);

        if (volume is null) throw new NotFoundException(ErrorMessages.ProductNotFound("Продукту"));

        volume.Quantity = request.NewQuantity;
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
