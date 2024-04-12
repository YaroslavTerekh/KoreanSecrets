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

namespace KoreanSecrets.BL.Behaviors.UserSelf.UpdatePassword;

public class UpdatePasswordHandler : IRequestHandler<UpdatePasswordCommand>
{
    private readonly DataContext _context;
    private readonly SignInManager<User> _signInManager;
    private readonly UserManager<User> _userManager;

    public UpdatePasswordHandler(DataContext context, SignInManager<User> signInManager, UserManager<User> userManager)
    {
        _context = context;
        _signInManager = signInManager;
        _userManager = userManager;

    }

    public async Task<Unit> Handle(UpdatePasswordCommand request, CancellationToken cancellationToken)
    {
        if (request.ConfirmPassword != request.Password) throw new Exception("Паролі не збігаються!");

        var user = await _context.Users.FirstOrDefaultAsync(t => t.Id == request.CurrentUserId, cancellationToken);

        var result = await _userManager.ChangePasswordAsync(user, request.OldPassword, request.Password);

        if (!result.Succeeded) throw new Exception(result.Errors.Select(t => t.Description).ToList().ToString());

        return Unit.Value;
    }
}
