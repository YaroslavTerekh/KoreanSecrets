using KoreanSecrets.Domain.Common.Constants;
using KoreanSecrets.Domain.Common.CustomExceptions;
using KoreanSecrets.Domain.Common.Enums;
using KoreanSecrets.Domain.DbConnection;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KoreanSecrets.BL.Behaviors.Admin.Products.Discounts.RemoveDiscount;

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
        product.DiscountPriceStartDate = null;
        product.DiscountPriceEndDate = null;
        product.UseDiscountPrice = false;
        
        await _context.SaveChangesAsync(cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
