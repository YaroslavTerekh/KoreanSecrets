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
using KoreanSecrets.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace KoreanSecrets.BL.Behaviors.UserSelf.ChangeProductAmount;

public class ChangeProductAmountHandler : IRequestHandler<ChangeProductAmountCommand>
{
    private readonly DataContext _context;
    private readonly UserManager<User> _userManager;
    public ChangeProductAmountHandler(DataContext context, UserManager<User> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task<Unit> Handle(ChangeProductAmountCommand request, CancellationToken cancellationToken)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(t => t.Id == request.CurrentUserId, cancellationToken);
        
        if (await CheckAdminRole(user))
        {
            var userBucket = 
                await _context.Users
                    .Where(t => t.Id == request.CurrentUserId)
                    .Include(t => t.AdminBucket)
                    .ThenInclude(t => t.BucketProducts)
                    .ThenInclude(t => t.Product)
                    .Select(t => t.AdminBucket)
                    .FirstOrDefaultAsync(cancellationToken);

            if (userBucket is null)
                throw new NotFoundException(ErrorMessages.UserNotFound);

            var purchaseProduct = userBucket.BucketProducts.FirstOrDefault(t => t.Id == request.PurchasedProductId);

            if (purchaseProduct is null)
                throw new Exception(ErrorMessages.PurchaseProductNotRelatedToUser);

            purchaseProduct = await _context.BucketProducts.FirstOrDefaultAsync(t => t.Id == request.PurchasedProductId);

            purchaseProduct.Amount = request.NewAmount;

            await _context.SaveChangesAsync(cancellationToken);
        }
        else
        {
            var userBucket = 
            
                await _context.Users
                    .Where(t => t.Id == request.CurrentUserId)
                    .Include(t => t.Bucket)
                    .ThenInclude(t => t.BucketProducts)
                    .ThenInclude(t => t.Product)
                    .Select(t => t.Bucket)
                    .FirstOrDefaultAsync(cancellationToken);

            if (userBucket is null)
                throw new NotFoundException(ErrorMessages.UserNotFound);

            var purchaseProduct = userBucket.BucketProducts.FirstOrDefault(t => t.Id == request.PurchasedProductId);

            if (purchaseProduct is null)
                throw new Exception(ErrorMessages.PurchaseProductNotRelatedToUser);

            purchaseProduct = await _context.BucketProducts.FirstOrDefaultAsync(t => t.Id == request.PurchasedProductId);

            purchaseProduct.Amount = request.NewAmount;

            await _context.SaveChangesAsync(cancellationToken);
        }

        return Unit.Value;
    }
    
    public async Task<bool> CheckAdminRole(User user)
    {
        return await _userManager.IsInRoleAsync(user, Roles.Admin);
    }
}
