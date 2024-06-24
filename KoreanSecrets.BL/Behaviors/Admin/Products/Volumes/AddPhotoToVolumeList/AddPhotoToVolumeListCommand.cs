using MediatR;
using Microsoft.AspNetCore.Http;

namespace KoreanSecrets.BL.Behaviors.Admin.Products.Volumes.AddPhotoToVolumeList;

public class AddPhotoToVolumeListCommand : IRequest
{
    public Guid VolumeId { get; set; }

    public IFormFile Photo { get; set; }
}
