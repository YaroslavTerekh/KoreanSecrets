using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoreanSecrets.BL.Behaviors.Admin.Promocodes.AddPromocode;

public class AddPromocodeCommand : IAuthorizedRequest
{
    public string Title { get; set; }

    public double Discount { get; set; }
}
