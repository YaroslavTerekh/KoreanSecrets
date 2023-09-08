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

namespace KoreanSecrets.BL.Behaviors.Products.DislikeProduct;

public class DislikeProductHandler : IRequestHandler<DislikeProductCommand>
{
    private readonly DataContext _context;

    public DislikeProductHandler(DataContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DislikeProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _context.Products.FirstOrDefaultAsync(t => t.Id == request.ProductId, cancellationToken);

        if (product is null)
            throw new NotFoundException(ErrorMessages.SomeProductNotFound);

        var user = await _context.Users
            .Include(t => t.Likes)
            .FirstOrDefaultAsync(t => t.Id == request.CurrentUserId, cancellationToken);

        if (user is null)
            throw new NotFoundException(ErrorMessages.SomeProductNotFound);

        if(user.Likes.Contains(product))
        {
            user.Likes.Remove(product);
            await _context.SaveChangesAsync(cancellationToken);
        }

        return Unit.Value;
    }
}
