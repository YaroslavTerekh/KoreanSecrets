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

namespace KoreanSecrets.BL.Behaviors.Admin.Products.AddDiscount;

public class AddDiscountHandler : IRequestHandler<AddDiscountCommand>
{
    private readonly DataContext _context;

    public AddDiscountHandler(DataContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(AddDiscountCommand request, CancellationToken cancellationToken)
    {
        var product = await _context.Products.FirstOrDefaultAsync(t => t.Id == request.ProductId, cancellationToken);

        if (product is null)
            throw new NotFoundException(ErrorMessages.SomeProductNotFound);

        product.DiscountPrice = request.NewPrice;
        product.AdditionalIcon = ProductIcon.Sale;

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
