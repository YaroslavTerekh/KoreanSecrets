using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoreanSecrets.BL.Behaviors.Admin.Demands.DeleteDemand;

public class DeleteDemandCommand : IRequest
{
    public Guid DemandId { get; set; }

    public DeleteDemandCommand(Guid id) => DemandId = id;
}
