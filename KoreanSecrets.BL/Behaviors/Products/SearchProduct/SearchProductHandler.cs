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

namespace KoreanSecrets.BL.Behaviors.Products.SearchProduct;

public class SearchProductHandler : IRequestHandler<SearchProductQuery, List<ListProductDTO>>
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;

    public SearchProductHandler(DataContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<ListProductDTO>> Handle(SearchProductQuery request, CancellationToken cancellationToken)
    {
        return await _context.Products
            .Where(t => t.Title.Contains(request.SearchText))
            .Select(t => _mapper.Map<ListProductDTO>(t))
            .ToListAsync(cancellationToken);
    }
}
