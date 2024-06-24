namespace KoreanSecrets.Domain.Entities.Banners;

public class Banner : BaseEntity
{
    public string Text { get; set; }

    public Guid BannerPhotoId { get; set; }

    public AppFile BannerPhoto { get; set; }

    public Guid BrandId { get; set; }

    public Brand Brand { get; set; }
}
