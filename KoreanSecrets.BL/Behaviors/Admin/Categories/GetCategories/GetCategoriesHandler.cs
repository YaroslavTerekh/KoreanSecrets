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

namespace KoreanSecrets.BL.Behaviors.Admin.Categories.GetCategories;

public class GetCategoriesHandler : IRequestHandler<GetCategoriesQuery, PaginationModelDTO<CategoryDTO>>
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;

    public GetCategoriesHandler(DataContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginationModelDTO<CategoryDTO>> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Categories.AsQueryable();

        query = request.Desc ? query.OrderByDescending(t => t.Title) : query;

        return new PaginationModelDTO<CategoryDTO>
        {
            CurrentPage = request.CurrentPage,
            PageSize = request.PageSize,
            Total = await query.CountAsync(cancellationToken),
            Products = await query.Skip(request.CurrentPage * request.PageSize).Take(request.PageSize).Select(t => _mapper.Map<CategoryDTO>(t)).ToListAsync(cancellationToken),
        };
    }
}
