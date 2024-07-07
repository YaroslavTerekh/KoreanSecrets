using KoreanSecrets.Domain.DataTransferObjects;
using MediatR;

namespace KoreanSecrets.BL.Behaviors.Admin.Users.UserInfo.GetUser;

public class GetUserQuery : IRequest<UserDTO>
{
    public Guid UserId { get; set; }
}
