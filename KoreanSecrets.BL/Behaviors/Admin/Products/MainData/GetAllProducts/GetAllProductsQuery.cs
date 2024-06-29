using KoreanSecrets.Domain.DataTransferObjects;
using MediatR;

namespace KoreanSecrets.BL.Behaviors.Admin.Products.MainData.GetAllProducts;

public class GetAllProductsQuery : IRequest<PaginationModelDTO<PageProductDTO>>
{
    public int CurrentPage { get; set; }

    public int PageSize { get; set; }
    
    public string? Text { get; set; }
    
    public string? WayToSort { get; set; }
    
    public string? ColumnToSort { get; set; }
    public List<string>? Brands { get; set; }
}
