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

namespace KoreanSecrets.BL.Behaviors.UserSelf.AddProductToBucket;

public class AddProductToBucketHandler : IRequestHandler<AddProductToBucketCommand>
{
    private readonly DataContext _context;

    public AddProductToBucketHandler(DataContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(AddProductToBucketCommand request, CancellationToken cancellationToken)
    {
        var product = await _context.Products
            .FirstOrDefaultAsync(t => t.Id == request.ProductId, cancellationToken);

        if (product is null)
            throw new NotFoundException(ErrorMessages.SomeProductNotFound);

        if (!product.IsInStock)
            throw new Exception(ErrorMessages.ProductNotInStock);

        var user = await _context.Users
            .Include(t => t.Bucket)
                .ThenInclude(t => t.Products)
            .FirstOrDefaultAsync(t => t.Id == request.CurrentUserId, cancellationToken);

        if (user is null)
            throw new NotFoundException(ErrorMessages.UserNotFound);

        user.Bucket.Products.Add(product);
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
