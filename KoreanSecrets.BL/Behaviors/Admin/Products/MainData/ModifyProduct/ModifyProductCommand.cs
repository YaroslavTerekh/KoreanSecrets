using System.Text.Json.Serialization;
using KoreanSecrets.Domain.Common.Enums;
using MediatR;

namespace KoreanSecrets.BL.Behaviors.Admin.Products.MainData.ModifyProduct;

public class ModifyProductCommand : IRequest
{
    [JsonIgnore]
    public Guid ProductId { get; set; }

    public string Title { get; set; }

    public string? Characteristics { get; set; }

    public string? Usage { get; set; }

    public string? Syllabes { get; set; }

    public Guid? CategoryId { get; set; }

    public Guid? BrandId { get; set; }

    public Guid? SubCategoryId { get; set; }

    public Guid? CountryId { get; set; }

    public List<Guid>? DemandId { get; set; }

    public ProductIcon Icon { get; set; } = ProductIcon.None;
}
