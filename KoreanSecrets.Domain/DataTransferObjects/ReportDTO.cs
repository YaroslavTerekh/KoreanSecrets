using KoreanSecrets.Domain.Common.Enums;
using KoreanSecrets.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoreanSecrets.Domain.DataTransferObjects;

public class ReportDTO : BaseEntity
{
    public string ReportText { get; set; }

    public ReportStatus Status { get; set; }

    public UserDTO User { get; set; }
}
