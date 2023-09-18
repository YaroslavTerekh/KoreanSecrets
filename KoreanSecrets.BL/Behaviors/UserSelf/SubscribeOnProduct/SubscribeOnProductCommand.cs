using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoreanSecrets.BL.Behaviors.UserSelf.SubscribeOnProduct;

public class SubscribeOnProductCommand : IAuthorizedRequest
{
    public Guid ProductId { get; set; }

    public SubscribeOnProductCommand(Guid productId, Guid userId)
    {
        ProductId = productId;
        CurrentUserId = userId;
    }
}
