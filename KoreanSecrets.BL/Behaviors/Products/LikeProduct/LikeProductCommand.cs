using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace KoreanSecrets.BL.Behaviors.Products.LikeProduct;

public class LikeProductCommand : IAuthorizedRequest
{
    [JsonIgnore]
    public Guid ProductId { get; set; }

    public LikeProductCommand(Guid id, Guid currentUserId)
    {
        ProductId = id;
        CurrentUserId = currentUserId;
    }
}
