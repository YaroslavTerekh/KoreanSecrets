using KoreanSecrets.Domain.Common.Constants;
using KoreanSecrets.Domain.Common.CustomExceptions;
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

namespace KoreanSecrets.BL.Behaviors.UserSelf.AddProductToBucket;

public class AddProductToBucketHandler : IRequestHandler<AddProductToBucketCommand>
{
    private readonly DataContext _context;
    private readonly UserManager<User> _userManager;

    public AddProductToBucketHandler(DataContext context, UserManager<User> userManager)
    {
        _context = context;
        _userManager = userManager;
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
                .ThenInclude(t => t.BucketProducts)
            .Include(t => t.AdminBucket)
                .ThenInclude(t => t.BucketProducts)
            .FirstOrDefaultAsync(t => t.Id == request.CurrentUserId, cancellationToken);

        if (user is null)
            throw new NotFoundException(ErrorMessages.UserNotFound);

        var volume = await _context.Volume
            .FirstOrDefaultAsync(t => t.Id == request.VolumeId, cancellationToken);

        if (volume is null)
            throw new NotFoundException(ErrorMessages.ProductNotFound("Об'єкту об'єму"));

        if (user?.Bucket?.BucketProducts?
            .FirstOrDefault(x => x.ProductId == request.ProductId && x.VolumeId == request.VolumeId) != null ||
            user?.AdminBucket?.BucketProducts?
            .FirstOrDefault(x => x.ProductId == request.ProductId && x.VolumeId == request.VolumeId) != null)
        {
            return Unit.Value;
        }

        var purchaseProduct = new BucketProduct
        {
            Amount = request.Amount,
            ProductId = product.Id,
            VolumeId = request.VolumeId,
        };
        
        if (await CheckAdminRole(user))
        {
            if (user.AdminBucket.Id == null)
            {
                var adminBucket = new AdminBucket()
                {
                    UserId = user.Id
                };
                
                purchaseProduct.AdminBucketId = adminBucket.Id;   
                await _context.AdminBuckets.AddAsync(adminBucket, cancellationToken);
            }
            else
            {
                purchaseProduct.AdminBucketId = user.AdminBucket.Id;   
            }
        }
        else if (user.BucketId != null)
        {
            purchaseProduct.BucketId = user.BucketId;
        }

        // volume.Quantity -= request.Amount;

        await _context.BucketProducts.AddAsync(purchaseProduct, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
    
    public async Task<bool> CheckAdminRole(User user)
    {
        return await _userManager.IsInRoleAsync(user, Roles.Admin);
    }
}
