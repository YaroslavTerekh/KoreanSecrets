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

namespace KoreanSecrets.BL.Behaviors.Admin.Countries.GetCountries;

public class GetCountriesHandler : IRequestHandler<GetCountriesQuery, PaginationModelDTO<CountryDTO>>
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;

    public GetCountriesHandler(DataContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginationModelDTO<CountryDTO>> Handle(GetCountriesQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Countries.AsQueryable();

        query = request.Desc ? query.OrderByDescending(t => t.Title) : query;

        return new PaginationModelDTO<CountryDTO>
        {
            CurrentPage = request.CurrentPage,
            PageSize = request.PageSize,
            Total = await query.CountAsync(cancellationToken),
            Products = await query.Select(t => _mapper.Map<CountryDTO>(t)).ToListAsync(cancellationToken),
        };
    }
}
