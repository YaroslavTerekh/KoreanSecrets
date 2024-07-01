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
            var productIds = order.Products.Select(t => Guid.Parse(t.ProductIdentify)).ToList();
            var products = await _context.Products.Where(t => productIds.Contains(t.Id)).ToListAsync(cancellationToken);

            foreach (var product in products)
            {
                foreach (var purchasedProduct in order.Products)
                {
                    if (Guid.Parse(purchasedProduct.ProductIdentify) == product.Id)
                    {
                        var volume = await _context.Volume.FirstOrDefaultAsync(t => t.Id == Guid.Parse(purchasedProduct.VolumeIdentify), cancellationToken);

                        if (volume is null)
                            throw new NotFoundException(ErrorMessages.VolumeNotFound);

                        volume.Quantity -= purchasedProduct.Amount;

                        break;
                    }
                }
            }
        }

        if (request.Status != PurchaseStatus.Failure && order.PurchaseStatus == PurchaseStatus.Failure)
        {
            var productIds = order.Products.Select(t => Guid.Parse(t.ProductIdentify)).ToList();
            var products = await _context.Products.Where(t => productIds.Contains(t.Id)).ToListAsync(cancellationToken);

            foreach (var product in products)
            {
                foreach (var purchasedProduct in order.Products)
                {
                    if (Guid.Parse(purchasedProduct.ProductIdentify) == product.Id)
                    {
                        var volume = await _context.Volume.FirstOrDefaultAsync(t => t.Id == Guid.Parse(purchasedProduct.VolumeIdentify), cancellationToken);

                        if (volume is null)
                            throw new NotFoundException(ErrorMessages.VolumeNotFound);

                        if (purchasedProduct.Amount! > volume.Quantity)
                        {
                            volume.Quantity -= purchasedProduct.Amount;
                            if (volume.Quantity <= 0) product.IsInStock = false;
                        }
                        break;
                    }
                }
            }
        }

        order.PurchaseStatus = request.Status;
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
