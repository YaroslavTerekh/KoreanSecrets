using KoreanSecrets.Domain.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoreanSecrets.BL.Behaviors.UserSelf.UpdateOrderStatus;

public class UpdateOrderStatusCommand : IAuthorizedRequest
{
    public Guid Id { get; set; }

    public PurchaseStatus Status { get; set; }
}
