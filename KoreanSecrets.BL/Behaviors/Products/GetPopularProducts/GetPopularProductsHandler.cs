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

namespace KoreanSecrets.BL.Behaviors.Products.GetPopularProducts;

public class GetPopularProductsHandler : IRequestHandler<GetPopularProductsQuery, List<ListProductDTO>>
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;

    public GetPopularProductsHandler(DataContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<ListProductDTO>> Handle(GetPopularProductsQuery request, CancellationToken cancellationToken)
    {
        return await _context.Products
            .Include(t => t.MainPhoto)
            .Include(t => t.Brand)
            .Include(t => t.Feedbacks)
            .OrderByDescending(t => t.Feedbacks.Count)
            .Skip(request.PageSize * request.CurrentPage)
            .Take(request.PageSize)
            .Select(t => _mapper.Map<ListProductDTO>(t))
            .ToListAsync(cancellationToken);
    }
}
