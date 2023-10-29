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

namespace KoreanSecrets.BL.Behaviors.Admin.Common.GetBrandsWithNullCategory;

public class GetBrandsWithNullCategoryHandler : IRequestHandler<GetBrandsWithNullCategoryQuery, AdminPaginationNullProperties<Brand>>
{
    private readonly DataContext _context;

    public GetBrandsWithNullCategoryHandler(DataContext context)
    {
        _context = context;
    }

    public async Task<AdminPaginationNullProperties<Brand>> Handle(GetBrandsWithNullCategoryQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Brands.Include(t => t.CategoryBrands).Where(t => t.CategoryBrands.Count < 1);

        query = request.Desc ? query.OrderByDescending(t => t.Title) : query;

        var entities = await query
            .Skip(request.CurrentPage * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);

        return new AdminPaginationNullProperties<Brand>
        {
            PageSize = request.PageSize,
            CurrentPage = request.CurrentPage,
            Total = await query.CountAsync(cancellationToken),
            Entities = entities
        };
    }
}
