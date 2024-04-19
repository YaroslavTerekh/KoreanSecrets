using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoreanSecrets.BL.Behaviors.UserSelf.AddReport;

public class AddReportCommand : IAuthorizedRequest
{
    public string Text { get; set; }
}
