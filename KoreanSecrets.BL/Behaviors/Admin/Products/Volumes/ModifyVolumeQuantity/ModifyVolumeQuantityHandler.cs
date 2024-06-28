using KoreanSecrets.Domain.Common.Constants;
using KoreanSecrets.Domain.DbConnection;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KoreanSecrets.BL.Behaviors.Admin.Products.Volumes.ModifyVolumeQuantity;

public class ModifyVolumeQuantityHandler : IRequestHandler<ModifyVolumeQuantityCommand>
{
    private readonly DataContext _context;

    public ModifyVolumeQuantityHandler(DataContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(ModifyVolumeQuantityCommand request, CancellationToken cancellationToken)
    {
        var volume = await _context.Volume
            .FirstOrDefaultAsync(t => t.Id == request.Id, cancellationToken);

        if (volume is null) throw new Exception(ErrorMessages.ProductNotFound("Об'єкту об'єму для модифікації"));
        
        volume.Quantity = request.Quantity;

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
