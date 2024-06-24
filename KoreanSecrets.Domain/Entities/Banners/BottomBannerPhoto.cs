namespace KoreanSecrets.Domain.Entities.Banners;

public class BottomBannerPhoto : BaseEntity
{
    public Guid PhotoId { get; set; }

    public AppFile Photo { get; set; }

    public Guid BottomBannerId { get; set; }
    
    public BottomBanner BottomBanner { get; set; }
    
    public bool IsSmall { get; set; }
}