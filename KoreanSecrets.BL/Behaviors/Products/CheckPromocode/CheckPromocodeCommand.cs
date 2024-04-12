using KoreanSecrets.Domain.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoreanSecrets.BL.Behaviors.Products.CheckPromocode;

public class CheckPromocodeCommand : IRequest<PromocodeUI?>
{
    public string Promocode { get; set; }
}
