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

namespace KoreanSecrets.BL.Behaviors.Admin.Products.ChangeIsInStockStatus;

public class ChangeIsInStockStatusHandler : IRequestHandler<ChangeIsInStockStatusCommand>
{
    private readonly DataContext _context;

    public ChangeIsInStockStatusHandler(DataContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(ChangeIsInStockStatusCommand request, CancellationToken cancellationToken)
    {
        var product = await _context.Products.FirstOrDefaultAsync(t => t.Id == request.ProductId, cancellationToken);

        if (product is null)
            throw new NotFoundException(ErrorMessages.SomeProductNotFound);

        product.IsInStock = !product.IsInStock;

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
