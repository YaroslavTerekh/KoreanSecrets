namespace KoreanSecrets.Domain.Entities.Banners;

public class BottomBanner : BaseEntity
{
    public List<BottomBannerPhoto> Photos { get; set; }
    
    public int Order { get; set; }
}
 