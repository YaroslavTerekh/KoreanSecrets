using System.Net;
using KoreanSecrets.BL.Services.Abstractions;
using KoreanSecrets.Domain.Common.Constants;
using KoreanSecrets.Domain.Common.CustomExceptions;
using KoreanSecrets.Domain.DbConnection;
using KoreanSecrets.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace KoreanSecrets.BL.Behaviors.Auth.ForgotPasswordConfirmChange;

public class ForgotPasswordConfirmChangeHandler : IRequestHandler<ForgotPasswordConfirmChangeCommand>
{
    private readonly UserManager<User> _userManager;
    private readonly DataContext _context;
    private readonly IPhoneNumberService _phoneNumberService;

    public ForgotPasswordConfirmChangeHandler(UserManager<User> userManager, DataContext context, IPhoneNumberService phoneNumberService)
    {
        _phoneNumberService = phoneNumberService;
        _userManager = userManager;
        _context = context;
    }

    public async Task<Unit> Handle(ForgotPasswordConfirmChangeCommand request, CancellationToken cancellationToken)
    {
        var user = await _context.Users.FirstOrDefaultAsync(t => t.PhoneNumber == _phoneNumberService.FormatPhoneNumber(request.PhoneNumber), cancellationToken);

        if (user is null)
            throw new NotFoundException(ErrorMessages.UserNotFound);

        if (user.TemporaryCode == null)
            throw new Exception(ErrorMessages.CodeExpired);

        if (user.TemporaryCode != request.ConfirmationCode)
            throw new Exception(ErrorMessages.CodeNotValid);

        var token = await _userManager.GenerateChangePhoneNumberTokenAsync(user, user.PhoneNumber);
        var result = await _userManager.ResetPasswordAsync(user, token, request.NewPassword);

        if (!result.Succeeded)
            throw new AuthException(HttpStatusCode.BadRequest, result.Errors);
        
        return Unit.Value;
    }
}
