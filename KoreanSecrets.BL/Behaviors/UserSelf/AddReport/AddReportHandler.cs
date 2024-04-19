using KoreanSecrets.Domain.Common.Enums;
using KoreanSecrets.Domain.DbConnection;
using KoreanSecrets.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoreanSecrets.BL.Behaviors.UserSelf.AddReport;

public class AddReportHandler : IRequestHandler<AddReportCommand>
{
    private readonly DataContext _context;

    public AddReportHandler(DataContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(AddReportCommand request, CancellationToken cancellationToken)
    {
        var report = new Report
        {
            UserId = request.CurrentUserId,
            Status = ReportStatus.Awaiting,
            ReportText = request.Text
        };

        await _context.Reports.AddAsync(report, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
