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
        var reports = _context.Reports
            .Include(t => t.User)
            .ThenInclude(x=>x.AddressInfo)
            .AsQueryable();
        
        if (!string.IsNullOrEmpty(request.ColumnToSort))
        {
            reports = request.ColumnToSort switch
            {
                "status" => request?.WayToSort == "asc"
                    ? reports.OrderBy(t => t.Status)
                    : reports.OrderByDescending(t => t.Status),
                "text" => request?.WayToSort == "asc"
                    ? reports.OrderBy(t => t.ReportText)
                    : reports.OrderByDescending(t => t.ReportText),
                "user" => request?.WayToSort == "asc"
                    ? reports.OrderBy(t => t.User.FirstName)
                    : reports.OrderByDescending(t => t.User.FirstName),
                "phone" => request?.WayToSort == "asc"
                    ? reports.OrderBy(t => t.User.PhoneNumber)
                    : reports.OrderByDescending(t => t.User.PhoneNumber),
                "email" => request?.WayToSort == "asc"
                    ? reports.OrderBy(t => t.User.Email)
                    : reports.OrderByDescending(t => t.User.Email),
                _ => reports
            };
        }
        else
        {
            reports = reports.OrderByDescending(x => x.CreatedDate);
        }

        return await reports.Select(t => _mapper.Map<ReportDTO>(t))
            .ToListAsync(cancellationToken);
    }
}
