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
            .ThenInclude(x=>x.Brand)
            .Include(t => t.Photos)
            .Include(t => t.UsersWaitingForStock)
            .ThenInclude(x=>x.User)
            .Where(t => t.UsersWaitingForStock.Any() && !t.IsInStock)
            .AsQueryable();

        if (!string.IsNullOrEmpty(request.ColumnToSort))
        {
            volume = request.ColumnToSort switch
            {
                "date" => request?.WayToSort == "asc"
                    ? volume.OrderBy(t => t.CreatedDate)
                    : volume.OrderByDescending(t => t.CreatedDate),
                "volume" => request?.WayToSort == "asc"
                    ? volume.OrderBy(t => t.Value)
                    : volume.OrderByDescending(t => t.Value),
                "photo" => request?.WayToSort == "asc"
                    ? volume.OrderBy(t => t.Product.Title)
                    : volume.OrderByDescending(t => t.Product.Title),
                "brand" => request?.WayToSort == "asc"
                    ? volume.OrderBy(t => t.Product.Brand.Title)
                    : volume.OrderByDescending(t => t.Product.Brand.Title),
                _ => volume
            };
        }
        else
        {
            volume = volume.OrderByDescending(x => x.CreatedDate);
        }
        
        return new PaginationModelDTO<VolumeExtendedDTO>()
        {
            Products = await volume
                .Skip(request.CurrentPage * request.PageSize)
                .Take(request.PageSize)
                .Select(t => _mapper.Map<VolumeExtendedDTO>(t))
                .ToListAsync(cancellationToken),
            Total = await volume.CountAsync(cancellationToken)
        };
    }
}
