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

namespace KoreanSecrets.BL.Behaviors.Admin.Common.GetDemandsWithNullCategory;

public class GetDemandsWithNullCategoryHandler : IRequestHandler<GetDemandsWithNullCategoryQuery, AdminPaginationNullProperties<Demand>>
{
    private readonly DataContext _context;

    public GetDemandsWithNullCategoryHandler(DataContext context)
    {
        _context = context;
    }

    public async Task<AdminPaginationNullProperties<Demand>> Handle(GetDemandsWithNullCategoryQuery request, CancellationToken cancellationToken)
    {
        //var query = _context.Demands.Include(t => t.CategoryDemands).Where(t => t.CategoryDemands.Count < 1);

        //query = request.Desc ? query.OrderByDescending(t => t.Title) : query;

        //var entities = await query
        //    .Skip(request.CurrentPage * request.PageSize)
        //    .Take(request.PageSize)
        //    .ToListAsync(cancellationToken);

        return new AdminPaginationNullProperties<Demand>
        {
            PageSize = request.PageSize,
            CurrentPage = request.CurrentPage,
            Total = 999,
            Entities = null
        };
    }
}
