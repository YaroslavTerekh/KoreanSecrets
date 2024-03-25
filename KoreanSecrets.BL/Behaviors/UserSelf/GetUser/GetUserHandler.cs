using KoreanSecrets.Domain.DbConnection;
using KoreanSecrets.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoreanSecrets.BL.Behaviors.UserSelf.GetUser;

public class GetUserHandler : IRequestHandler<GetUserQuery, User>
{
    private readonly DataContext _context;

    public GetUserHandler(DataContext context)
    {
        _context = context;
    }

    public async Task<User> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {
        return await _context.Users.FirstOrDefaultAsync(t => t.Id == request.CurrentUserId);
    }
}
