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

namespace KoreanSecrets.BL.Behaviors.Banners.GetAllBanners;

public class GetAllBannersHandler : IRequestHandler<GetAllBannersQuery, List<BannerDTO>>
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;

    public GetAllBannersHandler(DataContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<BannerDTO>> Handle(GetAllBannersQuery request, CancellationToken cancellationToken)
    {
        var banners = await _context.Banners
            .Include(t => t.BannerPhoto)
            .Select(t => _mapper.Map<BannerDTO>(t))
            .ToListAsync(cancellationToken);

        return banners;
    }
}
