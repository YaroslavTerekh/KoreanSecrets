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

namespace KoreanSecrets.BL.Behaviors.Admin.Products.ChangeVolumeIsInStock;

public class ChangeVolumeIsInStockHandler : IRequestHandler<ChangeVolumeIsInStockCommand>
{
    private readonly DataContext _context;

    public ChangeVolumeIsInStockHandler(DataContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(ChangeVolumeIsInStockCommand request, CancellationToken cancellationToken)
    {
        var volume = await _context.Volume.FirstOrDefaultAsync(t => t.Id == request.VolumeId, cancellationToken);

        if (volume is null)
            throw new NotFoundException(ErrorMessages.VolumeNotFound);

        volume.IsInStock = !volume.IsInStock;
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
