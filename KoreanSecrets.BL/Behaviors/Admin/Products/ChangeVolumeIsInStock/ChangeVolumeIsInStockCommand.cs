using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoreanSecrets.BL.Behaviors.Admin.Products.ChangeVolumeIsInStock;

public class ChangeVolumeIsInStockCommand : IRequest
{
    public Guid VolumeId { get; set; }
}
