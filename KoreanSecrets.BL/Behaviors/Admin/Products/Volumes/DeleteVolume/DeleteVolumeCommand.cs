using MediatR;

namespace KoreanSecrets.BL.Behaviors.Admin.Products.Volumes.DeleteVolume;

public class DeleteVolumeCommand : IRequest
{
    public Guid VolumeId { get; set; }

    public DeleteVolumeCommand(Guid id) => VolumeId = id;
}
