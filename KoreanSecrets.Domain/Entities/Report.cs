using KoreanSecrets.Domain.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoreanSecrets.Domain.Entities;

public class Report : BaseEntity
{
    public string ReportText { get; set; }

    public ReportStatus Status { get; set; }
    
    public Guid UserId { get; set; }

    public User User { get; set; }
}
