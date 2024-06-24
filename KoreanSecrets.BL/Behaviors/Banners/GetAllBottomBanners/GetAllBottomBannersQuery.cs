using KoreanSecrets.Domain.DataTransferObjects;
using KoreanSecrets.Domain.DataTransferObjects.Banners;
using MediatR;

namespace KoreanSecrets.BL.Behaviors.Banners.GetAllBottomBanners;

public class GetAllBottomBannersQuery : IRequest<List<BottomBannerDTO>>
{
}
