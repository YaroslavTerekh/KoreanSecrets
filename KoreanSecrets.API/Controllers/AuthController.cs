using KoreanSecrets.BL.Behaviors.Auth.ChangePassword;
using KoreanSecrets.BL.Behaviors.Auth.ConfirmPasswordChangeVerificationCode;
using KoreanSecrets.BL.Behaviors.Auth.ConfirmPhoneVerificationCode;
using KoreanSecrets.BL.Behaviors.Auth.ForgotPasswordConfirmChange;
using KoreanSecrets.BL.Behaviors.Auth.Login;
using KoreanSecrets.BL.Behaviors.Auth.Register;
using KoreanSecrets.BL.Behaviors.Auth.SendCodeRequestPassword;
using KoreanSecrets.BL.Behaviors.Auth.SendPhoneVerificationCode;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KoreanSecrets.API.Controllers;

[Route("api/auth")]
[ApiController]
public class AuthController : BaseController
{
    private readonly IMediator _mediatr;

    public AuthController(IMediator mediatr)
    {
        _mediatr = mediatr;
    }

    [HttpPost("register")]
    public async Task<IActionResult> RegisterUserAsync
        ([FromBody] RegisterCommand command, CancellationToken cancellationToken = default) =>
        Ok(await _mediatr.Send(command, cancellationToken));

    [HttpPost("login")]
    public async Task<IActionResult> LoginUserAsync
        ([FromBody] LoginCommand command, CancellationToken cancellationToken = default) =>
        Ok(await _mediatr.Send(command, cancellationToken));

    [Authorize]
    [HttpPatch("password/reset")]
    public async Task<IActionResult> ChangePasswordAsync
    (
        [FromBody] ChangePasswordCommand command,
        CancellationToken cancellationToken = default
    )
    {
        command.UserId = CurrentUserId;

        return Ok(await _mediatr.Send(command, cancellationToken));
    }

    [HttpPost("phone-code-request")]
    public async Task<IActionResult> PhoneCodeRequestAsync([FromBody] SendPhoneVerificationCodeCommand command,
        CancellationToken cancellationToken = default)
        => Ok(await _mediatr.Send(command, cancellationToken));

    [HttpPost("phone-code-confirm")]
    public async Task<IActionResult> PhoneCodeConfirmAsync([FromBody] ConfirmPhoneVerificationCodeCommand command,
        CancellationToken cancellationToken = default)
        => Ok(await _mediatr.Send(command, cancellationToken));


    // Password


    [HttpPost("code-request-password")]
    public async Task<IActionResult> CodeRequestPasswordAsync([FromBody] SendCodeRequestPasswordCommand command,
        CancellationToken cancellationToken = default)
        => Ok(await _mediatr.Send(command, cancellationToken));

    [HttpPost("code-request-password-confirm")]
    public async Task<IActionResult> CodePasswordConfirmAsync(
        [FromBody] ConfirmPasswordChangeVerificationCodeCommand command, CancellationToken cancellationToken = default)
        => Ok(await _mediatr.Send(command, cancellationToken));

    [HttpPost("confirm-forgot-password-change")]
    public async Task<IActionResult> ForgotPasswordConfirmChangeAsync([FromBody] ForgotPasswordConfirmChangeCommand command,
        CancellationToken cancellationToken = default)
        => Ok(await _mediatr.Send(command, cancellationToken));
}