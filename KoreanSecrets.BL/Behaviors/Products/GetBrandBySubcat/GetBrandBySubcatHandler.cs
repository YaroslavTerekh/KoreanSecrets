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

namespace KoreanSecrets.BL.Behaviors.Products.GetBrandBySubcat;

public class GetBrandBySubcatHandler : IRequestHandler<GetBrandBySubcatQuery, List<CategoryDTO>>
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;

    public GetBrandBySubcatHandler(DataContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<CategoryDTO>> Handle(GetBrandBySubcatQuery request, CancellationToken cancellationToken)
    {
        var entities = _context.Products
            .Include(t => t.Category)
            .Include(t => t.Brand)
            .Where(t => t.Brand.Id == request.BrandId && t.Category != null)
            .Select(t => _mapper.Map<CategoryDTO>(t.Category));

        return await entities.Distinct().ToListAsync(cancellationToken);

    }
}
