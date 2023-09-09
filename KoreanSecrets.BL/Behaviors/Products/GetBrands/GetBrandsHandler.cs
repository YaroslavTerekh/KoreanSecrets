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

namespace KoreanSecrets.BL.Behaviors.Products.GetBrands;

public class GetBrandsHandler : IRequestHandler<GetBrandsQuery, PaginationModelDTO<BrandDTO>>
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;

    public GetBrandsHandler(DataContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginationModelDTO<BrandDTO>> Handle(GetBrandsQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Brands
            .Include(t => t.Photo);

        return new PaginationModelDTO<BrandDTO>
        {
            CurrentPage = request.CurrentPage,
            PageSize = request.PageSize,
            Total = await query.CountAsync(cancellationToken),
            Products = await query.Select(t => _mapper.Map<BrandDTO>(t)).ToListAsync(cancellationToken)
        };
    }
}
