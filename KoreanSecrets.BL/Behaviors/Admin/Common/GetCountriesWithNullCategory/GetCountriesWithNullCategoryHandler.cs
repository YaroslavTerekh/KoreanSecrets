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

namespace KoreanSecrets.BL.Behaviors.Admin.Common.GetCountriesWithNullCategory;

public class GetCountriesWithNullCategoryHandler : IRequestHandler<GetCountriesWithNullCategoryQuery, AdminPaginationNullProperties<Country>>
{
    private readonly DataContext _context;

    public GetCountriesWithNullCategoryHandler(DataContext context)
    {
        _context = context;
    }

    public async Task<AdminPaginationNullProperties<Country>> Handle(GetCountriesWithNullCategoryQuery request, CancellationToken cancellationToken)
    {
        //var query = _context.Countries.Include(t => t.CategoryCountries).Where(t => t.CategoryCountries.Count < 1);

        //query = request.Desc ? query.OrderByDescending(t => t.Title) : query;

        //var entities = await query
        //    .Skip(request.CurrentPage * request.PageSize)
        //    .Take(request.PageSize)
        //    .ToListAsync(cancellationToken);

        return new AdminPaginationNullProperties<Country>
        {
            PageSize = request.PageSize,
            CurrentPage = request.CurrentPage,
            Total = 999,
            Entities = null
        };
    }
}
