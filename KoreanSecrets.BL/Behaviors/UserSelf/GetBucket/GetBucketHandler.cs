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
using KoreanSecrets.BL.Services;

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
        var bucketData = await _context.Users
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
                        .ThenInclude(x=>x.Photos)
            .Where(t => t.Id == request.CurrentUserId)
            .Select(t => t.Bucket)
            .FirstOrDefaultAsync(cancellationToken);

        if (bucketData?.BucketProducts != null)
            foreach (var product in bucketData.BucketProducts.Where(product => !product.Product.IsInStock || product.Volume.Quantity <= 0))
            {
                if (product.Amount > 0)
                {
                    product.Amount = 0;

                    _context.BucketProducts.Update(product);
                    await _context.SaveChangesAsync(cancellationToken);
                }
            }

        var bucket = _mapper.Map<BucketDTO>(bucketData);
        
        if (bucket is null)
            throw new NotFoundException(ErrorMessages.UserNotFound);

        var productBrandIds = bucket.PurchaseProducts.Select(t => t.Product.BrandId).ToList();
        var promotions = await _context.Promotions.Where(t => productBrandIds.Contains(t.BrandId)).ToListAsync(cancellationToken);
        
        foreach( var product in bucket.PurchaseProducts)
        {
            CalculatePriceService.GetProductPrice(product, promotions, null);
            
            if(product.Product.Brand is not null)
            {
                product.Product.Brand.Promotions = await _context.Promotions.FirstOrDefaultAsync(t => t.BrandId == product.Product.Brand.Id, cancellationToken);                
            }
        }

        return bucket;
    }
}
