using AutoMapper;
using KoreanSecrets.Domain.Common.Constants;
using KoreanSecrets.Domain.Common.CustomExceptions;
using KoreanSecrets.Domain.DataTransferObjects;
using KoreanSecrets.Domain.DbConnection;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KoreanSecrets.BL.Behaviors.UserSelf.SubscribeOnProductData.GetSubscriptions;

public class GetSubscriptionsHandler : IRequestHandler<GetSubscriptionsQuery, PaginationModelDTO<VolumeExtendedDTO>>
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;
    
    public GetSubscriptionsHandler(DataContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginationModelDTO<VolumeExtendedDTO>> Handle(GetSubscriptionsQuery request, CancellationToken cancellationToken)
    {
        var volume = _context.Volume
            .Include(t => t.Product)
            .Include(t => t.Photos)
            .Include(t => t.UsersWaitingForStock)
                .ThenInclude(t => t.Volume)
                    .ThenInclude(t => t.Product)
            .Where(t => t.UsersWaitingForStock.Any() && !t.IsInStock)
            .Select(t => _mapper.Map<VolumeExtendedDTO>(t))
            .AsQueryable();

        return new PaginationModelDTO<VolumeExtendedDTO>()
        {
            Products = await volume
                .Skip(request.CurrentPage * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync(cancellationToken),
            Total = await volume.CountAsync(cancellationToken)
        };
    }
}
