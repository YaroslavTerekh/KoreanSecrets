using KoreanSecrets.Domain.Common.Constants;
using KoreanSecrets.Domain.Common.CustomExceptions;
using KoreanSecrets.Domain.Common.Enums;
using KoreanSecrets.Domain.DbConnection;
using KoreanSecrets.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoreanSecrets.BL.Behaviors.UserSelf.UpdateOrderStatus;

public class UpdateOrderStatusHandler : IRequestHandler<UpdateOrderStatusCommand>
{
    private readonly DataContext _context;
    private readonly UserManager<User> _userManager;

    public UpdateOrderStatusHandler(DataContext context, UserManager<User> roleManager)
    {
        _context = context;
        _userManager = roleManager;
    }

    public async Task<Unit> Handle(UpdateOrderStatusCommand request, CancellationToken cancellationToken)
    {
        var order = await _context.Purchases
            .Include(t => t.Products)
            .FirstOrDefaultAsync(t => t.Id == request.Id, cancellationToken);
        var currentUser = await _context.Users.FirstOrDefaultAsync(t => t.Id == request.CurrentUserId, cancellationToken);

        if (order is null) throw new NotFoundException(ErrorMessages.PurchaseNotFound);

        if (currentUser is null) throw new NotFoundException(ErrorMessages.UserNotFound);

        if (order.UserId != request.CurrentUserId && !await _userManager.IsInRoleAsync(currentUser, Roles.Admin)) throw new Exception(ErrorMessages.PurchaseNotRelatedToUser);

        if (request.Status == PurchaseStatus.Failure)
        {
            var productIds = order.Products.Select(t => t.ProductId).ToList();
            var products = await _context.Products.Where(t => productIds.Contains(t.Id)).ToListAsync(cancellationToken);

            foreach (var product in products)
            {
                int purchaseAmount = 0;

                foreach (var purchasedProduct in order.Products)
                {
                    if (purchasedProduct.ProductId == product.Id) purchaseAmount = purchasedProduct.Amount; break;
                }

                product.Quantity += purchaseAmount;
                if (product.Quantity >= 0) product.IsInStock = true;
            }
        }

        if (request.Status != PurchaseStatus.Failure && order.PurchaseStatus == PurchaseStatus.Failure)
        {
            var productIds = order.Products.Select(t => t.ProductId).ToList();
            var products = await _context.Products.Where(t => productIds.Contains(t.Id)).ToListAsync(cancellationToken);

            foreach (var product in products)
            {
                int purchaseAmount = 0;

                foreach (var purchasedProduct in order.Products)
                {
                    if (purchasedProduct.ProductId == product.Id) purchaseAmount = purchasedProduct.Amount; break;
                }

                if(purchaseAmount !> product.Quantity)
                {
                    product.Quantity -= purchaseAmount;
                    if (product.Quantity <= 0) product.IsInStock = false;
                }
            }
        }

        order.PurchaseStatus = request.Status;
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
