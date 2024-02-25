using KoreanSecrets.Domain.DataTransferObjects;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoreanSecrets.BL.Behaviors.Admin.Users.GetUsers;

public class GetUsersQuery : IRequest<PaginationModelDTO<UserDTO>>
{
    public int CurrentPage { get; set; }

    public int PageSize { get; set; }
}
