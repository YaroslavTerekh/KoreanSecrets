using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoreanSecrets.BL.Behaviors.Admin.Products.RemoveDiscount;

public class RemoveDiscountCommand : IRequest
{
    public Guid ProductId { get; set; }

    public RemoveDiscountCommand(Guid id) => ProductId = id;
}
