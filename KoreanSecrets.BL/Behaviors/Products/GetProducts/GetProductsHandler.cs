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

public class GetProductsHandler : IRequestHandler<GetProductsQuery, List<Product>>
{
    private readonly DataContext _context;

    public GetProductsHandler(DataContext context)
    {
        _context = context;
    }

    public async Task<List<Product>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
    {
        var products = _context.Products
            .AsNoTracking()
            .Where(t => t.CategoryId == request.CategoryId);

        if (request.CountriesIds.Count > 0) products = products.Where(t => request.CountriesIds.Contains(t.CountryId));
        if (request.SubCategoriesIds.Count > 0) products = products.Where(t => request.SubCategoriesIds.Contains(t.SubCategoryId));
        if (request.DemandsIds.Count > 0) products = products.Where(t => request.DemandsIds.Contains(t.DemandId));
        if (request.BrandsIds.Count > 0) products = products.Where(t => request.BrandsIds.Contains(t.BrandId));

        return await products
            .Skip(request.CurrentPage * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);
    }
}
