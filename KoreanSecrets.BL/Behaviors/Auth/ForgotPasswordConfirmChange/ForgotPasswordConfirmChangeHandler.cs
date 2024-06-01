using System.Net;
using KoreanSecrets.Domain.Common.Constants;
using KoreanSecrets.Domain.Common.CustomExceptions;
using KoreanSecrets.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace KoreanSecrets.BL.Behaviors.Auth.ForgotPasswordConfirmChange;

public class ForgotPasswordConfirmChangeHandler : IRequestHandler<ForgotPasswordConfirmChangeCommand>
{
    private readonly UserManager<User> _userManager;

    public ForgotPasswordConfirmChangeHandler(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    public async Task<Unit> Handle(ForgotPasswordConfirmChangeCommand request, CancellationToken cancellationToken)
    {
        // Ще раз перевіряєш міняєш пароль або ерора
        
        // var user = await _userManager.FindByIdAsync(request.UserId.ToString());
        //
        // if (user is null)
        //     throw new NotFoundException(ErrorMessages.UserNotFound);
        //
        // var result = await _userManager.ChangePasswordAsync(user, request.OldPassword, request.NewPassword);
        //
        // if (!result.Succeeded)
        //     throw new AuthException(HttpStatusCode.BadRequest, result.Errors);
        //
        return Unit.Value;
    }
}
