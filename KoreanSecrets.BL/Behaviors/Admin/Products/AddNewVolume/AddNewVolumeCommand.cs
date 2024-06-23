using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoreanSecrets.BL.Behaviors.Admin.Products.AddNewVolume;

public class AddNewVolumeCommand : IRequest
{
    public Guid ProductId { get; set; }

    public string Value { get; set; }

    public string Unit { get; set; }

    public long Price { get; set; }

    public int Quantity { get; set; }
}
