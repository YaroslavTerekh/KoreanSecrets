using KoreanSecrets.BL.Services.Abstractions;
using KoreanSecrets.Domain.Common.Constants;
using KoreanSecrets.Domain.Common.CustomExceptions;
using KoreanSecrets.Domain.Entities;
using KoreanSecrets.Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace KoreanSecrets.BL.Behaviors.Auth.Register;

public class RegisterHandler : IRequestHandler<RegisterCommand>
{
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly IPhoneNumberService _phoneNumberService;

    public RegisterHandler(
        UserManager<User> userManager, 
        RoleManager<ApplicationRole> roleManager, 
        IPhoneNumberService phoneNumberService
    )
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _phoneNumberService = phoneNumberService;
    }

    public async Task<Unit> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var user = new User
        {
            UserName = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            PhoneNumber = _phoneNumberService.FormatPhoneNumber(request.PhoneNumber)
        };

        var userResult = await _userManager.CreateAsync(user, request.Password);

        if (!userResult.Succeeded)
            throw new AuthException(HttpStatusCode.BadRequest, userResult.Errors);

        var roleResult = await _userManager.AddToRoleAsync(user, Roles.User);

        if (!roleResult.Succeeded)
            throw new AuthException(HttpStatusCode.BadRequest, roleResult.Errors);

        return Unit.Value;
    }
}
