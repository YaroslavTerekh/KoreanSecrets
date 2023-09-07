using KoreanSecrets.Domain.Common.Constants;
using KoreanSecrets.Domain.Common.CustomExceptions;
using KoreanSecrets.Domain.Common.Enums;
using KoreanSecrets.Domain.DbConnection;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoreanSecrets.BL.Behaviors.Admin.Products.RemoveDiscount;

public class RemoveDiscountHandler : IRequestHandler<RemoveDiscountCommand>
{
    private readonly DataContext _context;

    public RemoveDiscountHandler(DataContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(RemoveDiscountCommand request, CancellationToken cancellationToken)
    {
        var product = await _context.Products.FirstOrDefaultAsync(t => t.Id == request.ProductId, cancellationToken);

        if (product is null)
            throw new NotFoundException(ErrorMessages.SomeProductNotFound);

        product.DiscountPrice = null;
        product.AdditionalIcon = ProductIcon.None;

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
