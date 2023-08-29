using KoreanSecrets.BL.Behaviors.Auth.Login;
using KoreanSecrets.BL.Behaviors.Auth.Register;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KoreanSecrets.API.Controllers;

[Route("api/auth")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediatr;

    public AuthController(IMediator mediatr)
    {
        _mediatr = mediatr;
    }

    [HttpPost("register")]
    public async Task<IActionResult> RegisterUserAsync
        (RegisterCommand command, CancellationToken cancellationToken = default) => Ok(await _mediatr.Send(command, cancellationToken));

    [HttpPost("login")]
    public async Task<IActionResult> LoginUserAsync
        (LoginCommand command, CancellationToken cancellationToken = default) => Ok(await _mediatr.Send(command, cancellationToken));
}
