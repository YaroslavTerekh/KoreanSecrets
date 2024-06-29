using KoreanSecrets.Domain.DataTransferObjects;
using KoreanSecrets.Domain.Entities;

namespace KoreanSecrets.BL.Behaviors.UserSelf.GePurchaseById;

public class GePurchaseByIdQuery : IAuthorizedRequest<Purchase>
{
    public long Id { get; set; }
}
