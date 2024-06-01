using KoreanSecrets.BL.Behaviors.Auth.ConfirmPhoneVerificationCode;
using KoreanSecrets.BL.Services.Abstractions;
using KoreanSecrets.Domain.Common.Constants;
using KoreanSecrets.Domain.Common.CustomExceptions;
using KoreanSecrets.Domain.DbConnection;
using KoreanSecrets.Domain.Entities;
using KoreanSecrets.Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace KoreanSecrets.BL.Behaviors.Auth.ConfirmPasswordChangeVerificationCode;

public class ConfirmPasswordChangeVerificationCodeHandler : IRequestHandler<ConfirmPasswordChangeVerificationCodeCommand, AuthToken>
{
    private readonly DataContext _context;
    private readonly IPhoneNumberService _phoneNumberService;
    private readonly UserManager<User> _userManager;
    private readonly IJWTService _jwtService;

    public ConfirmPasswordChangeVerificationCodeHandler(DataContext context, IPhoneNumberService phoneNumberService, UserManager<User> userManager, IJWTService jwtService)
    {
        _context = context;
        _phoneNumberService = phoneNumberService;
        _userManager = userManager;
        _jwtService = jwtService;
    }

    public async Task<AuthToken> Handle(ConfirmPasswordChangeVerificationCodeCommand request, CancellationToken cancellationToken)
    {
        // ТУТ перевіряєш код і шлеш мені ок чи погано
        
        // var user = await _context.Users.FirstOrDefaultAsync(t => t.Id == request.UserId, cancellationToken);
        //
        // if (user is null)
        //     throw new NotFoundException(ErrorMessages.UserNotFound);
        //
        // if (user.PhoneNumber != _phoneNumberService.FormatPhoneNumber(request.PhoneNumber))
        //     throw new Exception(ErrorMessages.WrongPhoneNumber);
        //
        // if (user.PhoneNumberConfirmed)
        //     throw new Exception(ErrorMessages.PhoneNumberAlreadyConfirmed);
        //
        // if (user.TemporaryCode != request.ComfirmationCode) throw new Exception(ErrorMessages.CodeNotValid); ;
        //
        // user.PhoneNumberConfirmed = true;
        // await _context.SaveChangesAsync(cancellationToken);
        //
        // var roles = await _userManager.GetRolesAsync(user);
        //
        // return _jwtService.GenerateJWT(user, roles.ToArray());

        return default;
    }
}
