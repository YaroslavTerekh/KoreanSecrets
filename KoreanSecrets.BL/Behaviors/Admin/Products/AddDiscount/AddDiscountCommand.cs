using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace KoreanSecrets.BL.Behaviors.Admin.Products.AddDiscount;

public class AddDiscountCommand : IRequest
{
    public Guid ProductId { get; set; }

    public long NewPrice { get; set; }

    public DateTime DiscountPriceStartDate { get; set; }

    public DateTime DiscountPriceEndDate { get; set; }
}
