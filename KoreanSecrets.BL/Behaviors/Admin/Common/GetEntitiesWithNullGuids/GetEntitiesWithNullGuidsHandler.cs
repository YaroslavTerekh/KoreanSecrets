using AutoMapper;
using KoreanSecrets.Domain.Common.Enums;
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

namespace KoreanSecrets.BL.Behaviors.Admin.Common.GetEntitiesWithNullGuids;

public class GetEntitiesWithNullGuidsHandler : IRequestHandler<GetEntitiesWithNullGuidsQuery, AdminPaginationNullProperties<Product>>
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;

    public GetEntitiesWithNullGuidsHandler(DataContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<AdminPaginationNullProperties<Product>> Handle(GetEntitiesWithNullGuidsQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Products.Where(t => t.CategoryId == null ||
                                                    t.CountryId == null ||
                                                    t.BrandId == null ||
                                                    t.SubCategoryId == null ||
                                                    t.DemandId == null);

        query = request.Desc ? query.OrderByDescending(t => t.Title) : query;

        var entities = await query
            .Skip(request.CurrentPage * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);

        return new AdminPaginationNullProperties<Product>
        {
            PageSize = request.PageSize,
            CurrentPage = request.CurrentPage,
            Total = await query.CountAsync(cancellationToken),
            Entities = entities
        };
    }
}
