using KoreanSecrets.Domain.Common.Enums;
using KoreanSecrets.Domain.DataTransferObjects;
using KoreanSecrets.Domain.Entities;
using MediatR;
using Newtonsoft.Json;

namespace KoreanSecrets.BL.Behaviors.Admin.Users.UserInfo.GetUserPurchases;

public class GetUserPurchasesQuery : IRequest<PaginationModelDTO<Purchase>>
{
    [JsonIgnore]
    public Guid UserId { get; set; }
    
    public int CurrentPage { get; set; }

    public int PageSize { get; set; }

    public PurchaseStatus? Status { get; set; }
    
    public string? WayToSort { get; set; }
    
    public string? ColumnToSort { get; set; }
}
