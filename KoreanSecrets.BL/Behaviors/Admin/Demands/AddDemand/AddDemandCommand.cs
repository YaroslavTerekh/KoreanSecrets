using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoreanSecrets.BL.Behaviors.Admin.Demands.AddDemand;

public class AddDemandCommand : IRequest
{
    public string Title { get; set; }
}
