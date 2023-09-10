using KoreanSecrets.BL.Services.Abstractions;
using KoreanSecrets.Domain.Common.Constants;
using KoreanSecrets.Domain.Common.CustomExceptions;
using KoreanSecrets.Domain.DbConnection;
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
    private readonly DataContext _context;

    public RegisterHandler(
        UserManager<User> userManager, 
        RoleManager<ApplicationRole> roleManager, 
        IPhoneNumberService phoneNumberService,
        DataContext context
    )
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _phoneNumberService = phoneNumberService;
        _context = context;
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

        var bucket = new Bucket
        {
            UserId = user.Id,
        };
        user.BucketId = bucket.Id;

        await _context.Buckets.AddAsync(bucket, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
