using AutoMapper;
using KoreanSecrets.Domain.DataTransferObjects;
using KoreanSecrets.Domain.DbConnection;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoreanSecrets.BL.Behaviors.Admin.SubCategories.GetSubCategories;

public class GetSubCategoriesHandler : IRequestHandler<GetSubCategoriesQuery, PaginationModelDTO<SubCategoryDTO>>
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;

    public GetSubCategoriesHandler(DataContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginationModelDTO<SubCategoryDTO>> Handle(GetSubCategoriesQuery request, CancellationToken cancellationToken)
    {
        var query = _context.SubCategories.AsQueryable();

        query = request.Desc ? query.OrderByDescending(t => t.Title) : query;

        return new PaginationModelDTO<SubCategoryDTO>
        {
            CurrentPage = request.CurrentPage,
            PageSize = request.PageSize,
            Total = await query.CountAsync(cancellationToken),
            Products = await query.Select(t => _mapper.Map<SubCategoryDTO>(t)).ToListAsync(cancellationToken),
        };
    }
}
