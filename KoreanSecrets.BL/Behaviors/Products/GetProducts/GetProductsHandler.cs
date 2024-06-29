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
using KoreanSecrets.BL.Services;
using KoreanSecrets.Domain.Common.Enums;

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
            .Include(t => t.Volumes)
            .ThenInclude(x=>x.Photos)
            .Include(t => t.MainPhoto)
            .AsQueryable();

        List<ListProductDTO> products = new();

        if(
            request.CountriesIds.Count < 1 &&
            request.SubCategoriesIds.Count < 1 &&
            request.DemandsIds.Count < 1 &&
            request.CategoriesIds.Count < 1 &&
            request.BrandsIds.Count < 1 &&
            !request.Sale &&
            !request.NewProduct &&
            string.IsNullOrEmpty(request.Text) && 
            string.IsNullOrWhiteSpace(request.Text)
        )
        {
            products = await query
                .Skip(request.CurrentPage * request.PageSize)
                .Take(request.PageSize)
                .Select(t => _mapper.Map<ListProductDTO>(t))
                .ToListAsync(cancellationToken);
            
            var productBrandIds = products.Select(t => t.BrandId).ToList();
            var promotions = await _context.Promotions.Where(t => productBrandIds.Contains(t.BrandId)).ToListAsync(cancellationToken);
            
            foreach (var product in products)
            {
                CalculatePriceService.GetProductPrice(product, promotions, null);
            }
            
            if (request.CurrentUserId != Guid.Empty)
            {
                foreach (var product in products)
                {
                    var likes = await _context.Products
                        .AsNoTracking()
                        .Where(t => product.Id == t.Id)
                        .SelectMany(t => t.Likes.Select(t => t.LikesId1)).ToListAsync(cancellationToken);

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

        if (!string.IsNullOrEmpty(request.Text) && !string.IsNullOrWhiteSpace(request.Text))
        {
            query = query.Where(t => t.Title.Contains(request.Text));
        }
        
        if (request.CountriesIds.Count > 0) query = query.Where(t => request.CountriesIds.Contains(t.CountryId!.Value));
        if (request.SubCategoriesIds.Count > 0) query = query.Where(t => request.SubCategoriesIds.Contains(t.SubCategoryId!.Value));
        if (request.DemandsIds.Count > 0) query = query.Where(t => t.ProductDemands.Select(t => t.DemandId).Any(id => request.DemandsIds.Contains(id)));
        if (request.BrandsIds.Count > 0) query = query.Where(t => request.BrandsIds.Contains(t.BrandId!.Value));
        if (request.CategoriesIds.Count > 0) query = query.Where(t => request.CategoriesIds.Contains(t.CategoryId!.Value));
        if (request.Sale) query = query.Where(t => t.AdditionalIcon == ProductIcon.Sale);
        if (request.NewProduct) query = query.Where(t => t.AdditionalIcon == ProductIcon.New);

        products = await query
                .Skip(request.CurrentPage * request.PageSize)
                .Take(request.PageSize)
                .Select(t => _mapper.Map<ListProductDTO>(t))
                .ToListAsync(cancellationToken);

        var productBrandIds1 = products.Select(t => t.BrandId).ToList();
        var promotions1 = await _context.Promotions.Where(t => productBrandIds1.Contains(t.BrandId)).ToListAsync(cancellationToken);
            
        foreach (var product in products)
        {
            CalculatePriceService.GetProductPrice(product, promotions1, null);
        }

        if (request.CurrentUserId != Guid.Empty)
        {
            foreach (var product in products)
            {
                var likes = await _context.Products
                    .AsNoTracking()
                    .Where(t => product.Id == t.Id)
                    .SelectMany(t => t.Likes.Select(t => t.LikesId1)).ToListAsync(cancellationToken);

                if (likes.Contains(request.CurrentUserId))
                    product.IsLikedByUser = true;
                else
                    product.IsLikedByUser = false;
            }
        }

        foreach (var product in products)
        {
            if (product.Brand is not null)
            {
                product.Brand.Promotions = await _context.Promotions.FirstOrDefaultAsync(t => t.BrandId == product.Brand.Id, cancellationToken);
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
