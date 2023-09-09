using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace KoreanSecrets.BL.Behaviors.Admin.Demands.ModifyDemand;

public class ModifyDemandCommand : IRequest
{
    [JsonIgnore]
    public Guid Id { get; set; }

    public string Title { get; set; }
}

