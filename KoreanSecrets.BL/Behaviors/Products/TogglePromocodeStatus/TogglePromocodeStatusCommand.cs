using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoreanSecrets.BL.Behaviors.Products.TogglePromocodeStatus;

public class TogglePromocodeStatusCommand : IRequest
{
    public Guid PromocodeId { get; set; }

    public TogglePromocodeStatusCommand(Guid id) => PromocodeId = id;
}
