using KoreanSecrets.Domain.DataTransferObjects;

namespace KoreanSecrets.BL.Behaviors.UserSelf.GetCountBucket;

public class GetCountBucketQuery : IAuthorizedRequest<int>
{
    public GetCountBucketQuery(Guid id)
    {
        CurrentUserId = id;
    }
}
