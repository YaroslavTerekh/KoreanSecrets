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

namespace KoreanSecrets.BL.Behaviors.Products.GetCategoriesBySubcat;

public class GetCategoriesBySubcatHandler : IRequestHandler<GetCategoriesBySubcatQuery, List<CategoryDTO>>
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;

    public GetCategoriesBySubcatHandler(DataContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<CategoryDTO>> Handle(GetCategoriesBySubcatQuery request, CancellationToken cancellationToken)
    {
        var entities = _context.Products
            .Include(t => t.Category)
            .Where(t => t.SubCategoryId == request.SubcategoryId)
            .Select(t => _mapper.Map<CategoryDTO>(t.Category));

        return await entities.DistinctBy(t => t.Id).ToListAsync(cancellationToken);
    }
}
