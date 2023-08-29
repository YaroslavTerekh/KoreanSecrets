using KoreanSecrets.Domain.Common.Constants;
using KoreanSecrets.Domain.Entities;
using KoreanSecrets.Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoreanSecrets.BL.Behaviors.Auth.Register;

public class RegisterHandler : IRequestHandler<RegisterCommand>
{
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<ApplicationRole> _roleManager;

    public RegisterHandler(UserManager<User> userManager, RoleManager<ApplicationRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task<Unit> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var user = new User
        {
            UserName = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            PhoneNumber = request.PhoneNumber
        };

        var result = await _userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
        {
            // TODO: Exceptions handling
            throw new Exception(result.Errors.ToList().ToString());
        }

        await _userManager.AddToRoleAsync(user, Roles.User);

        return Unit.Value;
    }
}
