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

namespace KoreanSecrets.BL.Behaviors.Products.GetSubCategoriesWithoutPagination;

public class GetSubCategoriesWithoutPaginationHandler : IRequestHandler<GetSubCategoriesWithoutPaginationQuery, List<SubCategoryDTO>>
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;

    public GetSubCategoriesWithoutPaginationHandler(DataContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<SubCategoryDTO>> Handle(GetSubCategoriesWithoutPaginationQuery request, CancellationToken cancellationToken)
    {
        return await _context.SubCategories.Select(t => _mapper.Map<SubCategoryDTO>(t)).ToListAsync(cancellationToken);
    }
}
