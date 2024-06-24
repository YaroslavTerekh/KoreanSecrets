using MediatR;

namespace KoreanSecrets.BL.Behaviors.Admin.BottomBanners.DeleteBanner;

public class DeleteBottomBannerCommand : IRequest
{
    public Guid BannerId { get; set; }

    public DeleteBottomBannerCommand(Guid id) => BannerId = id;
}
