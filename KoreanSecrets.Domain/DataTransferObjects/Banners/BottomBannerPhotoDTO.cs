using KoreanSecrets.Domain.Entities;
using KoreanSecrets.Domain.Entities.Banners;

namespace KoreanSecrets.Domain.DataTransferObjects.Banners;

public class BottomBannerPhotoDTO : BaseEntity
{
    public Guid PhotoId { get; set; }

    public AppFileDTO Photo { get; set; }

    public Guid BottomBannerId { get; set; }
    
    public BottomBannerDTO BottomBanner { get; set; }
    
    public bool IsSmall { get; set; }
}