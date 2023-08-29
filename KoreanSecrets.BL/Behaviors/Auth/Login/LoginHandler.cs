using KoreanSecrets.BL.Services.Abstractions;
using KoreanSecrets.Domain.DbConnection;
using KoreanSecrets.Domain.Entities;
using KoreanSecrets.Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoreanSecrets.BL.Behaviors.Auth.Login;

public class LoginHandler : IRequestHandler<LoginCommand, AuthToken>
{
    private readonly SignInManager<User> _signInManager;
    private readonly UserManager<User> _userManager;
    private readonly IJWTService _jwtService;
    private readonly DataContext _context;

    public LoginHandler(
        SignInManager<User> signInManager,
        UserManager<User> userManager,
        IJWTService jwtService,
        DataContext context
    )
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _jwtService = jwtService;
        _context = context;
    }
    public async Task<AuthToken> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await _context.Users.FirstOrDefaultAsync(t => t.PhoneNumber == request.PhoneNumber, cancellationToken);

        //if (user is null)
        //    throw new Exception(ErrorMessage.UserNotFound);

        //if (!user.PhoneNumberConfirmed)
        //    throw new Exception(ErrorMessages.PhoneNumberIsNotConfirmed);

        var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);

        if (!result.Succeeded)
            throw new Exception("asda");

        var roles = await _userManager.GetRolesAsync(user);

        return _jwtService.GenerateJWT(user, roles.ToArray());
    }
}
