using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoreanSecrets.BL.Behaviors.Admin.Products.ModifyVolume;

public class ModifyVolumeCommand : IRequest
{
    public Guid Id { get; set; }

    public string Value { get; set; }

    public string Unit { get; set; }

    public int Quantity { get; set; }

    public long Price { get; set; }
}
