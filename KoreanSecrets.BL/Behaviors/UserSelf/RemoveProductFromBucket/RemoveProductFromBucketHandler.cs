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

namespace KoreanSecrets.BL.Behaviors.UserSelf.RemoveProductFromBucket;

public class RemoveProductFromBucketHandler : IRequestHandler<RemoveProductFromBucketCommand>
{
    private readonly DataContext _context;
    private readonly UserManager<User> _userManager;
    
    public RemoveProductFromBucketHandler(DataContext context, UserManager<User> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task<Unit> Handle(RemoveProductFromBucketCommand request, CancellationToken cancellationToken)
    {
        var product = await _context.Products
            .FirstOrDefaultAsync(t => t.Id == request.ProductId, cancellationToken);

        if (product is null)
            throw new NotFoundException(ErrorMessages.SomeProductNotFound);

        var user = await _context.Users
            .Include(t => t.Bucket)
                .ThenInclude(t => t.BucketProducts)
            .Include(t => t.AdminBucket)
            .ThenInclude(t => t.BucketProducts)
            .FirstOrDefaultAsync(t => t.Id == request.CurrentUserId, cancellationToken);

        if (await CheckAdminRole(user))
        {
            if (user is null)
                throw new NotFoundException(ErrorMessages.UserNotFound);

            var purchaseProduct = user.AdminBucket.BucketProducts.Where(t => t.ProductId == request.ProductId).FirstOrDefault();

            if (purchaseProduct is null)
                throw new NotFoundException(ErrorMessages.SomeProductNotFound);

            var volume = await _context.Volume.FirstOrDefaultAsync(t => t.Id == purchaseProduct.VolumeId, cancellationToken);

            if (volume is null)
                throw new NotFoundException(ErrorMessages.VolumeNotFound);

            volume.Quantity += purchaseProduct.Amount;
            if (volume.Quantity >= 0) product.IsInStock = true;
            _context.BucketProducts.Remove(purchaseProduct);
            await _context.SaveChangesAsync(cancellationToken);
        }
        else {
            if (user is null)
                throw new NotFoundException(ErrorMessages.UserNotFound);

            var purchaseProduct = user.Bucket.BucketProducts.Where(t => t.ProductId == request.ProductId).FirstOrDefault();

            if (purchaseProduct is null)
                throw new NotFoundException(ErrorMessages.SomeProductNotFound);

            var volume = await _context.Volume.FirstOrDefaultAsync(t => t.Id == purchaseProduct.VolumeId, cancellationToken);

            if (volume is null)
                throw new NotFoundException(ErrorMessages.VolumeNotFound);

            volume.Quantity += purchaseProduct.Amount;
            if (volume.Quantity >= 0) product.IsInStock = true;
            _context.BucketProducts.Remove(purchaseProduct);
            await _context.SaveChangesAsync(cancellationToken);
            
        }

        return Unit.Value;
    }
    
    public async Task<bool> CheckAdminRole(User user)
    {
        return await _userManager.IsInRoleAsync(user, Roles.Admin);
    }
}
