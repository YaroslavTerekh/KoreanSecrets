using KoreanSecrets.Domain.DbConnection;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoreanSecrets.BL.Behaviors.UserSelf.UpdateUser;

public class UpdateUserHandler : IRequestHandler<UpdateUserCommand>
{
    private readonly DataContext _context;

    public UpdateUserHandler(DataContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _context.Users.FirstOrDefaultAsync(t => t.Id == request.CurrentUserId, cancellationToken);

        user.FirstName = request.FirstName;
        user.LastName = request.LastName;
        user.Email = request.Email;
        user.PhoneNumber = request.PhoneNumber;

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
