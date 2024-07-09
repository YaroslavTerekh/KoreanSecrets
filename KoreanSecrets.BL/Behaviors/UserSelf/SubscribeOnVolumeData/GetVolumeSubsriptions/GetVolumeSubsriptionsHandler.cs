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

namespace KoreanSecrets.BL.Behaviors.UserSelf.SubscribeOnVolumeData.GetVolumeSubsriptions;

public class GetVolumeSubsriptionsHandler : IRequestHandler<GetVolumeSubsriptionsQuery, PaginationModelDTO<VolumeDTO>>
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;

    public GetVolumeSubsriptionsHandler(DataContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginationModelDTO<VolumeDTO>> Handle(GetVolumeSubsriptionsQuery request, CancellationToken cancellationToken)
    {
        var volumes = _context.Volume
            .Include(t => t.UsersWaitingForStock)
            .Where(t => t.UsersWaitingForStock.Any() && !t.IsInStock)
            .Select(t => _mapper.Map<VolumeDTO>(t))
            .AsQueryable();

        return new PaginationModelDTO<VolumeDTO>()
        {
            Products = await volumes
                .Skip(request.CurrentPage * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync(cancellationToken),
            Total = await volumes.CountAsync(cancellationToken)
        };
    }
}
