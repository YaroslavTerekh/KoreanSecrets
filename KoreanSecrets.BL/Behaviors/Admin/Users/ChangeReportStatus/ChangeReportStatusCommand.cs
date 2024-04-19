using KoreanSecrets.Domain.Common.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoreanSecrets.BL.Behaviors.Admin.Users.ChangeReportStatus;

public class ChangeReportStatusCommand : IRequest
{
    public Guid ReportId { get; set; }

    public ReportStatus Status { get; set; }
}
