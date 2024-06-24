using KoreanSecrets.Domain.Common.Constants;
using KoreanSecrets.Domain.DbConnection;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KoreanSecrets.BL.Behaviors.Admin.Products.Volumes.ModifyVolume;

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
            .FirstOrDefaultAsync(t => t.Id == request.Id, cancellationToken);

        if (volume is null) throw new Exception(ErrorMessages.ProductNotFound("Об'єкту об'єму для модифікації"));

        volume.Price = request.Price;
        volume.Unit = request.Unit;
        volume.Value = request.Value;
        volume.Quantity = request.Quantity;

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
