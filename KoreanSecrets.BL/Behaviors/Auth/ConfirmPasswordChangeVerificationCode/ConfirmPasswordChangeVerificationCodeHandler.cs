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
        var user = await _context.Users.FirstOrDefaultAsync(t => t.PhoneNumber == request.PhoneNumber, cancellationToken);

        if (user is null)
            throw new NotFoundException(ErrorMessages.UserNotFound);

        if (user.TemporaryCode != request.ConfirmationCode) throw new Exception(ErrorMessages.CodeNotValid);

        var roles = await _userManager.GetRolesAsync(user);

        return _jwtService.GenerateJWT(user, roles.ToArray());
    }
}
