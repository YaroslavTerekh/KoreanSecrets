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

namespace KoreanSecrets.BL.Behaviors.Products.GetCategoriesWithoutPagination;

public class GetCategoriesWithoutPaginationHandler : IRequestHandler<GetCategoriesWithoutPaginationQuery, List<CategoryDTO>>
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;

    public GetCategoriesWithoutPaginationHandler(DataContext context, IMapper mepper)
    {
        _context = context; 
        _mapper = mepper;
    }

    public async Task<List<CategoryDTO>> Handle(GetCategoriesWithoutPaginationQuery request, CancellationToken cancellationToken)
    {
        return await _context.Categories.Select(t => _mapper.Map<CategoryDTO>(t)).ToListAsync(cancellationToken);
    }
}
