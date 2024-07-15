using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoreanSecrets.BL.Behaviors.Admin.Users.UserInfo.ModifyPaidDate;

public class ModifyPaidDateCommand : IRequest
{
    public Guid PurchaseId { get; set; }

    public DateTime PaidDate { get; set; }
}
