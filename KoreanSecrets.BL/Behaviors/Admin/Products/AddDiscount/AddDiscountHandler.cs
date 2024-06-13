using Hangfire;
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
using System.Threading;
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
        
        if (request.DiscountPriceStartDate > request.DiscountPriceEndDate)
            throw new Exception(ErrorMessages.DateNotMatch);

        if (request.DiscountPriceEndDate < DateTime.UtcNow)
            throw new Exception(ErrorMessages.DateNotMatch);

        product.DiscountPrice = request.NewPrice;
        product.DiscountPriceEndDate = request.DiscountPriceEndDate.AddHours(12);
        product.DiscountPriceStartDate = request.DiscountPriceStartDate.AddHours(12);

        await _context.SaveChangesAsync(cancellationToken);
        
        BackgroundJob.Schedule(() => SetIconAsync(product.Id, request.DiscountPriceEndDate, request.DiscountPriceStartDate), request.DiscountPriceStartDate - DateTime.UtcNow);
        BackgroundJob.Schedule(() => RemoveDiscountAsync(product.Id, request.DiscountPriceEndDate, request.DiscountPriceStartDate), request.DiscountPriceEndDate - DateTime.UtcNow);

        return Unit.Value;
    }

    public async Task SetIconAsync(Guid id, DateTime endDate, DateTime startDate)
    {
        var product = await _context.Products.FirstOrDefaultAsync(t => t.Id == id);

        if (product is null)
            return;

        if (endDate != product.DiscountPriceEndDate)
            return;

        if (startDate != product.DiscountPriceStartDate)
            return;

        product.AdditionalIcon = ProductIcon.Sale;
        product.UseDiscountPrice = true;
        
        await _context.SaveChangesAsync();
    }
    
    public async Task RemoveDiscountAsync(Guid id, DateTime endDate, DateTime startDate)
    {
        var product = await _context.Products.FirstOrDefaultAsync(t => t.Id == id);

        if (product is null)
            return;

        if (endDate != product.DiscountPriceEndDate)
            return;

        if (startDate != product.DiscountPriceStartDate)
            return;

        product.DiscountPriceStartDate = null;
        product.DiscountPriceEndDate = null;
        product.DiscountPrice = null;
        product.AdditionalIcon = ProductIcon.None;
        product.UseDiscountPrice = false;

        await _context.SaveChangesAsync();
    }
}
