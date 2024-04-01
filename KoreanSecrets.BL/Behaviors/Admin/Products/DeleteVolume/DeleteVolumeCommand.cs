using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoreanSecrets.BL.Behaviors.Admin.Products.DeleteVolume;

public class DeleteVolumeCommand : IRequest
{
    public Guid VolumeId { get; set; }

    public DeleteVolumeCommand(Guid id) => VolumeId = id;
}
