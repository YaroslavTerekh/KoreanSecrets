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

namespace KoreanSecrets.BL.Behaviors.UserSelf.SubscribeOnProduct;

public class SubscribeOnProductHandler : IRequestHandler<SubscribeOnProductCommand>
{
    private readonly DataContext _context;

    public SubscribeOnProductHandler(DataContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(SubscribeOnProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _context.Products
            .Include(t => t.UsersWaitingForStock)
            .FirstOrDefaultAsync(t => t.Id == request.ProductId, cancellationToken);

        if (product is null)
            throw new NotFoundException(ErrorMessages.SomeProductNotFound);

        var user = await _context.Users
            .FirstOrDefaultAsync(t => t.Id == request.CurrentUserId, cancellationToken);

        if (user is null)
            throw new NotFoundException(ErrorMessages.UserNotFound);

        product.UsersWaitingForStock.Add(user);
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
