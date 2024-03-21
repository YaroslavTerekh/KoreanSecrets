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

namespace KoreanSecrets.BL.Behaviors.Products.GetCountriesWithoutPagination;

public class GetCountriesWithoutPaginationHandler : IRequestHandler<GetCountriesWithoutPaginationQuery, List<CountryDTO>>
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;

    public GetCountriesWithoutPaginationHandler(DataContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<CountryDTO>> Handle(GetCountriesWithoutPaginationQuery request, CancellationToken cancellationToken)
    {
        return await _context.Countries.Select(t => _mapper.Map<CountryDTO>(t)).ToListAsync(cancellationToken);
    }
}
