using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoreanSecrets.BL.Behaviors.UserSelf.SubscribeOnVolumeData.SubscribeOnVolume;

public class SubscribeOnVolumeCommand : IAuthorizedRequest
{
    public Guid VolumeId { get; set; }

    public SubscribeOnVolumeCommand(Guid productId, Guid userId)
    {
        VolumeId = productId;
        CurrentUserId = userId;
    }
}
