using KoreanSecrets.Domain.Entities;

namespace KoreanSecrets.Domain.DataTransferObjects.Banners;

public class BottomBannerDTO : BaseEntity
{
    public List<BottomBannerPhotoDTO> Photos { get; set; }
    
    public int Order { get; set; }
}
 