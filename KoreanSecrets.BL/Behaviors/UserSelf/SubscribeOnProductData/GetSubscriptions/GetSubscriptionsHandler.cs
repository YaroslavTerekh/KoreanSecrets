using AutoMapper;
using KoreanSecrets.Domain.Common.Constants;
using KoreanSecrets.Domain.Common.CustomExceptions;
using KoreanSecrets.Domain.DataTransferObjects;
using KoreanSecrets.Domain.DbConnection;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KoreanSecrets.BL.Behaviors.UserSelf.SubscribeOnProductData.GetSubscriptions;

public class GetSubscriptionsHandler : IRequestHandler<GetSubscriptionsQuery, PaginationModelDTO<ListProductDTO>>
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;
    
    public GetSubscriptionsHandler(DataContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginationModelDTO<ListProductDTO>> Handle(GetSubscriptionsQuery request, CancellationToken cancellationToken)
    {
        var product = _context.Products
            .Include(t => t.UsersWaitingForStock)
            .Include(t => t.Brand)
            .Include(t => t.Volumes)
            .Where(t => t.UsersWaitingForStock.Any() && !t.IsInStock)
            .Select(t => _mapper.Map<ListProductDTO>(t))
            .AsQueryable();

        return new PaginationModelDTO<ListProductDTO>()
        {
            Products = await product
                .Skip(request.CurrentPage * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync(cancellationToken),
            Total = await product.CountAsync(cancellationToken)
        };
    }
}
