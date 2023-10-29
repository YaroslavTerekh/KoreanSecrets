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

namespace KoreanSecrets.BL.Behaviors.Admin.Demands.GetDemands;

public class GetDemandsHandler : IRequestHandler<GetDemandsQuery, PaginationModelDTO<DemandDTO>>
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;

    public GetDemandsHandler(DataContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginationModelDTO<DemandDTO>> Handle(GetDemandsQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Demands.AsQueryable();

        query = request.Desc ? query.OrderByDescending(t => t.Title) : query;

        return new PaginationModelDTO<DemandDTO>
        {
            CurrentPage = request.CurrentPage,
            PageSize = request.PageSize,
            Total = await query.CountAsync(cancellationToken),
            Products = await query.Select(t => _mapper.Map<DemandDTO>(t)).ToListAsync(cancellationToken),
        };
    }
}
