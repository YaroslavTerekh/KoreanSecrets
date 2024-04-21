using KoreanSecrets.Domain.DbConnection;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoreanSecrets.BL.Behaviors.Admin.Users.ChangeReportStatus;

public class ChangeReportStatusHandler : IRequestHandler<ChangeReportStatusCommand>
{
    private readonly DataContext _context;

    public ChangeReportStatusHandler(DataContext context)
    {
        _context = context;        
    }

    public async Task<Unit> Handle(ChangeReportStatusCommand request, CancellationToken cancellationToken)
    {
        var report = await _context.Reports.FirstOrDefaultAsync(t => t.Id == request.ReportId, cancellationToken);

        report.Status = request.Status;

        _context.Reports.Update(report);
        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
