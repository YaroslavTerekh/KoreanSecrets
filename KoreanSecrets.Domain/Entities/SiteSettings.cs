namespace KoreanSecrets.Domain.Entities;

public class SiteSettings
{
    public Guid Id { get; set; }
    public bool EnableLiqPay { get; set; } = true;
    public bool EnablePayByCard { get; set; } = true;
}