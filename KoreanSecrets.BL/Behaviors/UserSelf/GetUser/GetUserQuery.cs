using KoreanSecrets.Domain.DataTransferObjects;
using KoreanSecrets.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoreanSecrets.BL.Behaviors.UserSelf.GetUser;

public class GetUserQuery : IAuthorizedRequest<User>
{
    public GetUserQuery(Guid userId)
    {
        CurrentUserId = userId;
    }
}
