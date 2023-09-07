using AutoMapper;
using KoreanSecrets.Domain.Common.Constants;
using KoreanSecrets.Domain.Common.CustomExceptions;
using KoreanSecrets.Domain.DataTransferObjects;
using KoreanSecrets.Domain.DbConnection;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoreanSecrets.BL.Behaviors.Products.GetProduct;

public class GetProductHandler : IRequestHandler<GetProductQuery, PageProductDTO>
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;

    public GetProductHandler(DataContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PageProductDTO> Handle(GetProductQuery request, CancellationToken cancellationToken)
    {
        var product = await _context.Products
            .AsNoTracking()
            .Include(t => t.Brand)
            .Include(t => t.SubCategory)
            .Include(t => t.Country)
            .Include(t => t.Demand)
            .Include(t => t.Photos)
            .Include(t => t.Guide)
            .Include(t => t.MainPhoto)
            .Include(t => t.Feedbacks)
                .ThenInclude(t => t.User)
            .Include(t => t.Volumes)
            .Where(t => t.Id == request.ProductId)
            .Select(t => _mapper.Map<PageProductDTO>(t))
            .FirstOrDefaultAsync(cancellationToken);

        if (product is null)
            throw new NotFoundException(ErrorMessages.SomeProductNotFound);

        product.SameProducts = await _context.Products
            .AsNoTracking()
            .Include(t => t.MainPhoto)
            .Include(t => t.Brand)
            .Where(t => t.Id != product.Id && t.SubCategoryId == product.SubCategoryId)
            .OrderByDescending(t => t.CreatedDate)
            .Take(4)
            .Select(t => _mapper.Map<ListProductDTO>(t))
            .ToListAsync(cancellationToken);

        return product;
    }
}
