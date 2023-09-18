using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoreanSecrets.BL.Behaviors.UserSelf.AddProductToBucket;

public class AddProductToBucketCommand : IAuthorizedRequest
{
    public Guid ProductId { get; set; }

    public AddProductToBucketCommand(Guid productId, Guid currentUserId)
    {
        ProductId = productId;
        CurrentUserId = currentUserId;
    }
}
