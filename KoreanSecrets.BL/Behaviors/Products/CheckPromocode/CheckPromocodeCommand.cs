using KoreanSecrets.Domain.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace KoreanSecrets.BL.Behaviors.Products.CheckPromocode;

public class CheckPromocodeCommand : IRequest<PromocodeUI?>
{
    [JsonIgnore]
    public Guid CurrentUserId { get; set; }

    public string Promocode { get; set; }
}
