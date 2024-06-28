using AutoMapper;
using KoreanSecrets.Domain.Common.Constants;
using KoreanSecrets.Domain.Common.CustomExceptions;
using KoreanSecrets.Domain.DataTransferObjects;
using KoreanSecrets.Domain.DbConnection;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoreanSecrets.BL.Behaviors.Products.GetProduct;

public class GetProductHandler : IRequestHandler<GetProductQuery, PageProductDTO>
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;

    public GetProductHandler(DataContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PageProductDTO> Handle(GetProductQuery request, CancellationToken cancellationToken)
    {
        var product = await _context.Products
            .AsNoTracking()
            .Include(t => t.Brand)
            .Include(t => t.SubCategory)
            .Include(t => t.Country)
            .Include(t => t.Volumes)
            .ThenInclude(x=>x.Photos)
            .Include(t => t.ProductDemands)
                .ThenInclude(t => t.Demand)
            .Include(t => t.Category)
            .Include(t => t.Photos)
            .Include(t => t.Guide)
            .Include(t => t.Comments)
                .ThenInclude(t => t.User)
            .Include(t => t.Comments)
                .ThenInclude(t => t.Replies)
                    .ThenInclude(t => t.Replies)
            .Include(t => t.MainPhoto)
            .Include(t => t.Feedbacks.OrderByDescending(x=>x.CreatedDate))
                .ThenInclude(t => t.User)
            .Include(t => t.Volumes)
                .ThenInclude(x=>x.Photos)
            .Where(t => t.Id == request.ProductId)
            .FirstOrDefaultAsync(cancellationToken);

        product.Comments = product.Comments.Where(t => t.ParentCommentId == null).ToList();
        var mappedProduct = _mapper.Map<PageProductDTO>(product);

        if (product is null)
            throw new NotFoundException(ErrorMessages.SomeProductNotFound);

        if( product.Brand is not null)
        {
            var promotion = await _context.Promotions.FirstOrDefaultAsync(t => t.BrandId == product.Brand.Id, cancellationToken);

            if(promotion is not null)
            {
                mappedProduct.Brand.Promotions = promotion;
            }
        }

        if (request.CurrentUserId != Guid.Empty)
        {
            var likes = await _context.Products
                    .AsNoTracking()
                    .Where(t => product.Id == t.Id)
                    .SelectMany(t => t.Likes.Select(t => t.LikesId1)).ToListAsync(cancellationToken);

            if (likes.Contains(request.CurrentUserId))
                mappedProduct.IsLikedByUser = true;
            else
                mappedProduct.IsLikedByUser = false;
        }

        mappedProduct.SameProducts = await _context.Products
            .AsNoTracking()
            .Include(t => t.MainPhoto)
            .Include(t => t.Brand)
            .Include(t => t.Volumes)
            .ThenInclude(x=>x.Photos)
            .Where(t => t.Id != product.Id && t.SubCategoryId == product.SubCategoryId)
            .OrderByDescending(t => t.CreatedDate)
            .Take(4)
            .Select(t => _mapper.Map<ListProductDTO>(t))
            .ToListAsync(cancellationToken);

        return mappedProduct;
    }
}
