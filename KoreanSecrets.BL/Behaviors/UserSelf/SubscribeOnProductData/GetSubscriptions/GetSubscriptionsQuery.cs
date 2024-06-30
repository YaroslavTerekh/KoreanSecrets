using KoreanSecrets.Domain.DataTransferObjects;

namespace KoreanSecrets.BL.Behaviors.UserSelf.SubscribeOnProductData.GetSubscriptions;

public class GetSubscriptionsQuery : IAuthorizedRequest<PaginationModelDTO<ListProductDTO>>
{
    public int CurrentPage { get; set; }

    public int PageSize { get; set; }
    
    public string? Text { get; set; }
    
    public string? WayToSort { get; set; }
    
    public string? ColumnToSort { get; set; }
}
