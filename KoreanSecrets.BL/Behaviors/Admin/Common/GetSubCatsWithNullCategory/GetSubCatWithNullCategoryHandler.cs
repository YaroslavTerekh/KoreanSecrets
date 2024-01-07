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

namespace KoreanSecrets.BL.Behaviors.Admin.Common.GetSubCatsWithNullCategory;

public class GetSubCatWithNullCategoryHandler : IRequestHandler<GetSubCatWithNullCategoryQuery, AdminPaginationNullProperties<SubCategory>>
{
    private readonly DataContext _context;

    public GetSubCatWithNullCategoryHandler(DataContext context)
    {
        _context = context;
    }

    public async Task<AdminPaginationNullProperties<SubCategory>> Handle(GetSubCatWithNullCategoryQuery request, CancellationToken cancellationToken)
    {
        //var query = _context.SubCategories.Include(t => t.CategorySubCategories).Where(t => t.CategorySubCategories.Count < 1);

        //query = request.Desc ? query.OrderByDescending(t => t.Title) : query;

        //var entities = await query
        //    .Skip(request.CurrentPage * request.PageSize)
        //    .Take(request.PageSize)
        //    .ToListAsync(cancellationToken);

        return new AdminPaginationNullProperties<SubCategory>
        {
            PageSize = request.PageSize,
            CurrentPage = request.CurrentPage,
            Total = 999,
            Entities = null
        };
    }
}
