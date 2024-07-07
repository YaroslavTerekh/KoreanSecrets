using KoreanSecrets.Domain.Common.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoreanSecrets.BL.Behaviors.Purchases.GeneratePurchase;

public class GeneratePurchaseCommand : IAuthorizedRequest<object>
{
    public PayType PayType { get; set; }

    public string? Comment { get; set; }

    public string? Promocode { get; set; }

    public Address Address { get; set; }

    public bool SaveAddress { get; set; }
    
    public string UserInfo { get; set; }
    public string Phone { get; set; }
    public string? Email { get; set; }
}

public class Address
{
    public string City { get; set; }

    public string Warehouse { get; set; }
}
