using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoreanSecrets.BL.Behaviors.Admin.AddDemand;

public class AddDemandCommand : IRequest
{
    public Guid CategoryId { get; set; }

    public string Title { get; set; }
}
