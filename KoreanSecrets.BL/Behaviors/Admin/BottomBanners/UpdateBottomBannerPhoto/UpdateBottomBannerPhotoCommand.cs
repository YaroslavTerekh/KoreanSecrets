using MediatR;
using Microsoft.AspNetCore.Http;

namespace KoreanSecrets.BL.Behaviors.Admin.BottomBanners.UpdateBottomBannerPhoto;

public class UpdateBottomBannerPhotoCommand : IRequest
{
    public Guid BannerId { get; set; }
    public Guid PhotoId { get; set; }
    public IFormFile Photo { get; set; }
}
