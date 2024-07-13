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

namespace KoreanSecrets.BL.Behaviors.Auth.Login;

public class LoginHandler : IRequestHandler<LoginCommand, AuthToken>
{
    private readonly SignInManager<User> _signInManager;
    private readonly UserManager<User> _userManager;
    private readonly IJWTService _jwtService;
    private readonly DataContext _context;
    private readonly IPhoneNumberService _phoneNumberService;

    public LoginHandler(
        SignInManager<User> signInManager,
        UserManager<User> userManager,
        IJWTService jwtService,
        DataContext context,
        IPhoneNumberService phoneNumberService
    )
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _jwtService = jwtService;
        _context = context;
        _phoneNumberService = phoneNumberService;
    }
    public async Task<AuthToken> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(t => t.PhoneNumber == _phoneNumberService.FormatPhoneNumber(request.PhoneNumber), cancellationToken);

        if (user is null)
            throw new NotFoundException(ErrorMessages.UserNotFound);

        var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);

        if (!result.Succeeded)
            throw new AuthException(HttpStatusCode.BadRequest, ErrorMessages.WrongPassword);
        
        var bucket = await _context.Buckets
            .Include(bucket => bucket.BucketProducts)
            .FirstOrDefaultAsync(x => x.UserId == user.Id, cancellationToken);

        if (bucket != null)
        {

            var newProducts = new List<BucketProduct>();
            
            if (bucket?.BucketProducts == null)
            {
                bucket.BucketProducts = new List<BucketProduct>();
            }
        
            if (request.Bucket != null
                && request.Bucket.Any())
            {
                foreach (var product in bucket.BucketProducts)
                {
                    if (request.Bucket.Any(x => x.VolumeId == product.VolumeId && x.ProductId == product.ProductId))
                    {
                        product.Amount = request.Bucket
                            .First(x => x.VolumeId == product.VolumeId && x.ProductId == product.ProductId).Amount;
                        
                        _context.BucketProducts.Update(product);
                        await _context.SaveChangesAsync(cancellationToken);
                    } 
                }

                foreach (var product in request.Bucket.Where(x=> 
                             !bucket.BucketProducts.Any(b => x.VolumeId == b.VolumeId && x.ProductId == b.ProductId)))
                {
                    newProducts.Add(new BucketProduct()
                    {
                        ProductId = product.ProductId,
                        VolumeId = product.VolumeId,
                        Amount = product.Amount,
                        BucketId = bucket.Id
                    });
                }
            }

            if (newProducts.Any())
            {
                await _context.BucketProducts.AddRangeAsync(newProducts, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
            }
            
        }

        if(request.Likes.Count != null)
        {
            foreach (var product in request.Likes)
            {
                var like = new ProductUser
                {
                    LikesId1 = user.Id,
                    LikesId = product
                };

                if (!_context.ProductUser.Any(t => t.LikesId1 == like.LikesId1 && t.LikesId == like.LikesId))
                {
                    await _context.ProductUser.AddAsync(like);
                }
            }
            await _context.SaveChangesAsync(cancellationToken);
        }

        var roles = await _userManager.GetRolesAsync(user);

        return _jwtService.GenerateJWT(user, roles.ToArray());
    }
}
