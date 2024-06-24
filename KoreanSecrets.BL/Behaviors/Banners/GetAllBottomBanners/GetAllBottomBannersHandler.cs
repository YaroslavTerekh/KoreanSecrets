using AutoMapper;
using KoreanSecrets.Domain.DataTransferObjects;
using KoreanSecrets.Domain.DataTransferObjects.Banners;
using KoreanSecrets.Domain.DbConnection;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KoreanSecrets.BL.Behaviors.Banners.GetAllBottomBanners;

public class GetAllBottomBannersHandler : IRequestHandler<GetAllBottomBannersQuery, List<BottomBannerDTO>>
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;

    public GetAllBottomBannersHandler(DataContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<BottomBannerDTO>> Handle(GetAllBottomBannersQuery request, CancellationToken cancellationToken)
    {
        var banners = await _context.BottomBanners
            .Include(t => t.Photos)
            .ThenInclude(x=>x.Photo)
            .Select(t => _mapper.Map<BottomBannerDTO>(t))
            .ToListAsync(cancellationToken);

        return banners;
    }
}
