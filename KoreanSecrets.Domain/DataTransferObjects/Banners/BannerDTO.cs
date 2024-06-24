using KoreanSecrets.Domain.Entities;

namespace KoreanSecrets.Domain.DataTransferObjects.Banners;

public class BannerDTO : BaseEntity
{
    public string Text { get; set; }

    public AppFileDTO BannerPhoto { get; set; }

    public Guid BrandId { get; set; }
    public BrandDTO Brand { get; set; }
}
