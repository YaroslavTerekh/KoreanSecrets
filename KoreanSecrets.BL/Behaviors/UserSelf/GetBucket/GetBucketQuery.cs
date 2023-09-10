using KoreanSecrets.Domain.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoreanSecrets.BL.Behaviors.UserSelf.GetBucket;

public class GetBucketQuery : IAuthorizedRequest<BucketDTO>
{
    public GetBucketQuery(Guid id)
    {
        CurrentUserId = id;
    }
}
