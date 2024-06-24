using KoreanSecrets.Domain.DataTransferObjects;
using MediatR;

namespace KoreanSecrets.BL.Behaviors.Admin.Products.MainData.GetProducts;

public class GetAdminProductsQuery : IRequest<PaginationModelDTO<PageProductDTO>>
{
    public string SearchText { get; set; }

    public int CurrentPage { get; set; }

    public int PageSize { get; set; }
}
