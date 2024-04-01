using KoreanSecrets.Domain.Common.Constants;
using KoreanSecrets.Domain.DbConnection;
using KoreanSecrets.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoreanSecrets.BL.Behaviors.Admin.Products.ModifyVolume;

public class ModifyVolumeHandler : IRequestHandler<ModifyVolumeCommand>
{
    private readonly DataContext _context;

    public ModifyVolumeHandler(DataContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(ModifyVolumeCommand request, CancellationToken cancellationToken)
    {
        var volume = await _context.Volume
            .FirstOrDefaultAsync(t => t.Id == request.VolumeId, cancellationToken);

        if (volume is null) throw new Exception(ErrorMessages.ProductNotFound("Об'єкту об'єму для модифікації"));

        volume.Price = request.Price;
        volume.Unit = request.Unit;
        volume.Value = request.Value;

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
