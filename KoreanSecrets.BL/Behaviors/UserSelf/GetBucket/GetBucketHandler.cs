using AutoMapper;
using KoreanSecrets.Domain.Common.Constants;
using KoreanSecrets.Domain.Common.CustomExceptions;
using KoreanSecrets.Domain.DataTransferObjects;
using KoreanSecrets.Domain.DbConnection;
using KoreanSecrets.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoreanSecrets.BL.Behaviors.UserSelf.GetBucket;

public class GetBucketHandler : IRequestHandler<GetBucketQuery, BucketDTO>
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;

    public GetBucketHandler(DataContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<BucketDTO> Handle(GetBucketQuery request, CancellationToken cancellationToken)
    {
        var bucket = await _context.Users
            .Include(t => t.Bucket)
                .ThenInclude(t => t.BucketProducts)
                    .ThenInclude(t => t.Product)
                        .ThenInclude(t => t.MainPhoto)
            .Include(t => t.Bucket)
                .ThenInclude(t => t.BucketProducts)
                    .ThenInclude(t => t.Product)
                        .ThenInclude(t => t.Brand)
            .Include(t => t.Bucket)
                .ThenInclude(t => t.BucketProducts)
                    .ThenInclude(t => t.Volume)
            .Where(t => t.Id == request.CurrentUserId)
            .Select(t => _mapper.Map<BucketDTO>(t.Bucket))
            .FirstOrDefaultAsync(cancellationToken);

        if (bucket is null)
            throw new NotFoundException(ErrorMessages.UserNotFound);

        var productsIds = bucket.PurchaseProducts.Select(t => t.Product.Id).ToList();
        var products = await _context.Products.Where(t => productsIds.Contains(t.Id)).ToListAsync(cancellationToken);

        var lowQuantityIds = products.Where(t => t.Quantity <= 0).Select(t => t.Id).ToList();
        var purchasesToDeleteIds = bucket.PurchaseProducts.Where(t => lowQuantityIds.Contains(t.Product.Id)).Select(t => t.Id).ToList();

        var purchases = await _context.PurchasedProducts.Where(t => purchasesToDeleteIds.Contains(t.Id)).ToListAsync(cancellationToken);

        _context.PurchasedProducts.RemoveRange(purchases);

        return bucket;
    }
}
