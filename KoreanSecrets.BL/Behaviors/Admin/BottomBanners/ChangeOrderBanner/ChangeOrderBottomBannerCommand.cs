using MediatR;

namespace KoreanSecrets.BL.Behaviors.Admin.BottomBanners.ChangeOrderBanner;

public class ChangeOrderBottomBannerCommand : IRequest
{
    public Guid BannerId { get; set; }
    public int Order { get; set; }
    
}
