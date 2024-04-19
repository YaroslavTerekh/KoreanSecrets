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

namespace KoreanSecrets.BL.Behaviors.Admin.Users.GetReports;

public class GetReportsHandler : IRequestHandler<GetReportsQuery, List<ReportDTO>>
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;

    public GetReportsHandler(DataContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<ReportDTO>> Handle(GetReportsQuery request, CancellationToken cancellationToken)
    {
        var reports = await _context.Reports
            .Include(t => t.User)
            .Select(t => _mapper.Map<ReportDTO>(t))
            .ToListAsync(cancellationToken);

        return reports;
    }
}
