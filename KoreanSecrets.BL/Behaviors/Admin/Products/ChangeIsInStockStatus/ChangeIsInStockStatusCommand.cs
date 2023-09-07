using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoreanSecrets.BL.Behaviors.Admin.Products.ChangeIsInStockStatus;

public class ChangeIsInStockStatusCommand : IRequest
{
    public Guid ProductId { get; set; }

    public ChangeIsInStockStatusCommand(Guid id) => ProductId = id;
}
