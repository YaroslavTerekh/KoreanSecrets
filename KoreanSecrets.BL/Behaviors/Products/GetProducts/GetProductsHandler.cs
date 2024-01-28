using AutoMapper;
using KoreanSecrets.BL.Helpers;
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

namespace KoreanSecrets.BL.Behaviors.Products.GetProducts;

public class GetProductsHandler : IRequestHandler<GetProductsQuery, PaginationModelDTO<ListProductDTO>>
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;

    public GetProductsHandler(DataContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginationModelDTO<ListProductDTO>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Products
            .AsNoTracking()
            .Include(t => t.Brand)
            .Include(t => t.MainPhoto)
            .Where(t => t.CategoryId == request.CategoryId && t.CountryId != null && t.SubCategoryId != null && t.DemandId != null && t.BrandId != null);

        if (request.CountriesIds.Count > 0) query = query.Where(t => request.CountriesIds.Contains(t.CountryId!.Value));
        if (request.SubCategoriesIds.Count > 0) query = query.Where(t => request.SubCategoriesIds.Contains(t.SubCategoryId!.Value));
        if (request.DemandsIds.Count > 0) query = query.Where(t => request.DemandsIds.Contains(t.DemandId!.Value));
        if (request.BrandsIds.Count > 0) query = query.Where(t => request.BrandsIds.Contains(t.BrandId!.Value));

        var products = await query
                .Skip(request.CurrentPage * request.PageSize)
                .Take(request.PageSize)
                .Select(t => _mapper.Map<ListProductDTO>(t))
                .ToListAsync(cancellationToken);

        if (request.CurrentUserId != Guid.Empty)
        {
            foreach (var product in products)
            {
                var likes = await _context.Products
                    .AsNoTracking()
                    .Where(t => product.Id == t.Id)
                    .SelectMany(t => t.Likes.Select(t => t.Id)).ToListAsync(cancellationToken);

                if (likes.Contains(request.CurrentUserId))
                    product.IsLikedByUser = true;
                else
                    product.IsLikedByUser = false;
            }
        }

        return new PaginationModelDTO<ListProductDTO>
        {
            CurrentPage = request.CurrentPage,
            PageSize = request.PageSize,
            Total = await query.CountAsync(cancellationToken),
            Products = products
        };
    }
}
