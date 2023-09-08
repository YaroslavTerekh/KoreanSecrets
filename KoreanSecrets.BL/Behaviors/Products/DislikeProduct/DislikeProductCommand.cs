using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace KoreanSecrets.BL.Behaviors.Products.DislikeProduct;

public class DislikeProductCommand : IAuthorizedRequest
{
    [JsonIgnore]
    public Guid ProductId { get; set; }

    public DislikeProductCommand(Guid id, Guid currentUserId)
    {
        ProductId = id;
        CurrentUserId = currentUserId;
    }
}
