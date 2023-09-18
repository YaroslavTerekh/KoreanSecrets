using KoreanSecrets.Domain.Common.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoreanSecrets.BL.Behaviors.Purchases.GeneratePurchase;

public class GeneratePurchaseCommand : IAuthorizedRequest<string>
{
    public PayType PayType { get; set; }

    public string? Comment { get; set; }

    public string? Promocode { get; set; }
}
