using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoreanSecrets.BL.Behaviors.Admin.Products.ChangeProductQuantity;

public class ChangeProductQuantityCommand : IRequest
{
    public Guid VolumeId { get; set; }

    public int NewQuantity { get; set; }
}
