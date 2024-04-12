using KoreanSecrets.Domain.Common.Constants;
using KoreanSecrets.Domain.Common.CustomExceptions;
using KoreanSecrets.Domain.DbConnection;
using KoreanSecrets.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoreanSecrets.BL.Behaviors.UserSelf.UpdatePasswordUnauthorized;

public class UpdatePasswordUnauthorizedHandler : IRequestHandler<UpdatePasswordUnauthorizedCommand>
{
    private readonly DataContext _context;
    private readonly SignInManager<User> _signInManager;
    private readonly UserManager<User> _userManager;

    public UpdatePasswordUnauthorizedHandler(DataContext context, SignInManager<User> signInManager, UserManager<User> userManager)
    {
        _context = context;
        _signInManager = signInManager;
        _userManager = userManager;

    }

    public async Task<Unit> Handle(UpdatePasswordUnauthorizedCommand request, CancellationToken cancellationToken)
    {
        if (request.NewPassword != request.ConfirmNewPassword) throw new Exception("Паролі не збігаються!");

        var user = await _context.Users.FirstOrDefaultAsync(t => t.Email == request.Email, cancellationToken);

        if (user is null) throw new NotFoundException(ErrorMessages.UserNotFound);

        var result = await _userManager.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword);

        if (result.Succeeded) throw new Exception(result.Errors.Select(t => t.Description).ToList().ToString());

        return Unit.Value;
    }
}
