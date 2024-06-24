using MediatR;
using Microsoft.AspNetCore.Http;

namespace KoreanSecrets.BL.Behaviors.Admin.BottomBanners.AddBanner;

public class AddBottomBannerCommand : IRequest
{
    public IFormFile FirstPhoto { get; set; }
    public IFormFile SecondPhoto { get; set; }
    public int Order { get; set; }
}
