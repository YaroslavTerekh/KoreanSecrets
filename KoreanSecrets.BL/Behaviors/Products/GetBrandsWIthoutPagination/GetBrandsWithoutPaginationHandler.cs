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

namespace KoreanSecrets.BL.Behaviors.Products.GetBrandsWIthoutPagination;

public class GetBrandsWithoutPaginationHandler : IRequestHandler<GetBrandsWithoutPaginationQuery, List<BrandDTO>>
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;

    public GetBrandsWithoutPaginationHandler(DataContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<BrandDTO>> Handle(GetBrandsWithoutPaginationQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Brands
            .Include(t => t.Photo);

        return await query.Select(t => _mapper.Map<BrandDTO>(t)).ToListAsync(cancellationToken);
    }
}
