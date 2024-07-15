using KoreanSecrets.Domain.DataTransferObjects;

namespace KoreanSecrets.BL.Behaviors.Admin.GetAdminBucket;

public class GetAdminBucketQuery : IAuthorizedRequest<BucketDTO>
{
    public GetAdminBucketQuery(Guid id)
    {
        CurrentUserId = id;
    }
}
