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

public class GetSubCategoriesHandler : IRequestHandler<GetSubCategoriesQuery, List<SubCategoryDTO>>
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;

    public GetSubCategoriesHandler(DataContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<SubCategoryDTO>> Handle(GetSubCategoriesQuery request, CancellationToken cancellationToken)
    {
        var query = _context.SubCategories.AsQueryable();

        return await query.Select(t => _mapper.Map<SubCategoryDTO>(t)).ToListAsync(cancellationToken);
    }
}
