using Hangfire;
using KoreanSecrets.BL.Services.Abstractions;
using KoreanSecrets.Domain.Common.Constants;
using KoreanSecrets.Domain.Common.CustomExceptions;
using KoreanSecrets.Domain.DbConnection;
using KoreanSecrets.Domain.Entities;
using KoreanSecrets.Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace KoreanSecrets.BL.Behaviors.Auth.Register;

public class RegisterHandler : IRequestHandler<RegisterCommand, Guid>
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

    public async Task<Guid> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var user = new User
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            PhoneNumberConfirmed = false,
            Email = null,
            EmailConfirmed = false
        };

        user.UserName = Guid.NewGuid().ToString();

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

        if (request.Bucket != null
            && request.Bucket.Any())
        {
            var newProducts = new List<BucketProduct>();

            foreach (var product in request.Bucket)
            {
                newProducts.Add(new BucketProduct()
                {
                    ProductId = product.ProductId,
                    VolumeId = product.VolumeId,
                    Amount = product.Amount,
                    BucketId = bucket.Id
                });
            }

            if (newProducts.Any())
            {
                await _context.BucketProducts.AddRangeAsync(newProducts, cancellationToken);
            }
        }
        
        await _context.Buckets.AddAsync(bucket, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        BackgroundJob.Schedule(() => RemoveUser(user.Id, cancellationToken), TimeSpan.FromMinutes(10));

        return user.Id;
    }
    public async Task RemoveUser(Guid id, CancellationToken cancellationToken = default)
    {
        var user = await _context.Users.FirstOrDefaultAsync(t => t.Id == id, cancellationToken);

        if (user is null)
            throw new NotFoundException(ErrorMessages.UserNotFound);

        if (user.PhoneNumberConfirmed == false)
            await _userManager.DeleteAsync(user);      
    }
}
