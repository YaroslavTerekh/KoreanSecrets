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

public class GetAllProductsHandler : IRequestHandler<GetAllProductsQuery, List<ListProductDTO>>
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;

    public GetAllProductsHandler(DataContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<ListProductDTO>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
    {
        var products = await _context.Products
            .AsNoTracking()
            .Include(t => t.Brand)
            .Include(t => t.MainPhoto)
            .Select(t => _mapper.Map<ListProductDTO>(t))
            .Skip(request.CurrentPage * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);

        return products;
    }
}
