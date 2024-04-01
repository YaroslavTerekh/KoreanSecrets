using KoreanSecrets.Domain.Common.Constants;
using KoreanSecrets.Domain.DbConnection;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoreanSecrets.BL.Behaviors.Admin.Products.DeleteVolume;

public class DeleteVolumeHandler : IRequestHandler<DeleteVolumeCommand>
{
    private readonly DataContext _context;

    public DeleteVolumeHandler(DataContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteVolumeCommand request, CancellationToken cancellationToken)
    {
        var volume = await _context.Volume
            .FirstOrDefaultAsync(t => t.Id == request.VolumeId, cancellationToken);

        if (volume is null) throw new Exception(ErrorMessages.ProductNotFound("Об'єкту об'єму для видалення"));

        _context.Volume.Remove(volume);
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
