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

namespace KoreanSecrets.BL.Behaviors.Products.GetDemandsWithoutPagination;

public class GetDemandsWithoutPaginationHandler : IRequestHandler<GetDemandsWithoutPaginationQuery, List<DemandDTO>>
{
    private readonly IMapper _mapper;
    private readonly DataContext _context;

    public GetDemandsWithoutPaginationHandler(DataContext context, IMapper mapper)
    {
        _mapper = mapper;
        _context = context;
    }

    public async Task<List<DemandDTO>> Handle(GetDemandsWithoutPaginationQuery request, CancellationToken cancellationToken)
    {
        return await _context.Demands.Select(t => _mapper.Map<DemandDTO>(t)).ToListAsync(cancellationToken);
    }
}
