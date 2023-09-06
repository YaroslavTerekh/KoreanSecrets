using AutoMapper;
using KoreanSecrets.Domain.Common.Enums;
using KoreanSecrets.Domain.DataTransferObjects;
using KoreanSecrets.Domain.DbConnection;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoreanSecrets.BL.Behaviors.Products.GetDiscountedProducts;

public class GetDiscountedProductsHandler : IRequestHandler<GetDiscountedProductsQuery, PaginnationModelDTO<ListProductDTO>>
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;

    public GetDiscountedProductsHandler(DataContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginnationModelDTO<ListProductDTO>> Handle(GetDiscountedProductsQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Products
            .Include(t => t.Brand)
            .Include(t => t.MainPhoto)
            .Where(t => t.DiscountPrice != null && t.AdditionalIcon == ProductIcon.Sale);

        return new PaginnationModelDTO<ListProductDTO>
        {
            CurrentPage = request.CurrentPage,
            PageSize = request.PageSize,
            Total = await query.CountAsync(cancellationToken),
            Products = await query
            .Skip(request.CurrentPage * request.PageSize)
            .Take(request.PageSize)
            .Select(t => _mapper.Map<ListProductDTO>(t))
            .ToListAsync(cancellationToken)
    };
    }
}
