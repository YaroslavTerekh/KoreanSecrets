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

namespace KoreanSecrets.BL.Behaviors.Admin.Products.AddNewVolume;

public class AddNewVolumeHandler : IRequestHandler<AddNewVolumeCommand>
{
    private readonly DataContext _context;

    public AddNewVolumeHandler(DataContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(AddNewVolumeCommand request, CancellationToken cancellationToken)
    {
        var product = await _context.Products
            .Include(t => t.Volumes)
            .FirstOrDefaultAsync(t => t.Id == request.ProductId, cancellationToken);

        if (product is null) throw new Exception(ErrorMessages.ProductNotFound("Продукту для модифікації"));

        var volume = new Volume
        {
            ProductId = request.ProductId,
            Price = request.Price,
            Unit = request.Unit,
            Value = request.Value,
            Quantity = request.Quantity
        };

        await _context.Volume.AddAsync(volume, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
