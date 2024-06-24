using KoreanSecrets.BL.Behaviors.Admin.Products.Volumes.AddNewVolume;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace KoreanSecrets.BL.Behaviors.Admin.Products.Volumes.AddNewVolumes;

public class AddNewVolumesCommand : IRequest
{
    public List<AddNewVolumeCommand> Data { get; set; }
}
