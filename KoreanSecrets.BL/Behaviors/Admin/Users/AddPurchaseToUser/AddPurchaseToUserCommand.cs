using KoreanSecrets.BL.Behaviors.Purchases.GeneratePurchase;
using KoreanSecrets.Domain.Common.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoreanSecrets.BL.Behaviors.Admin.Users.AddPurchaseToUser;

public class AddPurchaseToUserCommand : IAuthorizedRequest
{
    public string? Comment { get; set; }

    public string? Promocode { get; set; }

    public Address Address { get; set; }

    public bool SaveAddress { get; set; }

    public string UserInfo { get; set; }

    public string Phone { get; set; }

    public string? Email { get; set; }

    public Guid? UserId { get; set; }
}
