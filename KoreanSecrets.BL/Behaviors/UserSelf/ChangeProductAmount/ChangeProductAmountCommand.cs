using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoreanSecrets.BL.Behaviors.UserSelf.ChangeProductAmount;

public class ChangeProductAmountCommand : IAuthorizedRequest
{
    public Guid PurchasedProductId { get; set; }

    public int NewAmount { get; set; }
}
