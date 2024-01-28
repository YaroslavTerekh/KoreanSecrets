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

namespace KoreanSecrets.BL.Behaviors.Admin.Products.GetAllProducts;

public class GetAllProductsHandler : IRequestHandler<GetAllProductsQuery, PaginationModelDTO<ListProductDTO>>
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;

    public GetAllProductsHandler(DataContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginationModelDTO<ListProductDTO>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Products
            .AsNoTracking()
            .Where(t => t.BrandId != null && t.CategoryId != null && t.CountryId != null && t.DemandId != null && t.SubCategoryId != null)
            .Include(t => t.Brand)
            .Include(t => t.MainPhoto);

        return new PaginationModelDTO<ListProductDTO>
        {
            CurrentPage = request.CurrentPage,
            PageSize = request.PageSize,
            Total = await query.CountAsync(cancellationToken),
            Products = await query
                .Select(t => _mapper.Map<ListProductDTO>(t))
                .Skip(request.CurrentPage * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync(cancellationToken)
        };
    }
}
