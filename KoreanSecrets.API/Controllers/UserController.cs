using KoreanSecrets.BL.Behaviors.UserSelf.AddFeedback;
using KoreanSecrets.BL.Behaviors.UserSelf.SubscribeOnProduct;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KoreanSecrets.API.Controllers;

[Authorize]
[Route("api/user")]
[ApiController]
public class UserController : BaseController
{
    private readonly IMediator _mediatr;

    public UserController(IMediator mediator)
    {
        _mediatr = mediator;
    }

    [HttpPost("feedbacks/add")]
    public async Task<IActionResult> AddFeedbackAsync
    (
        [FromBody] AddFeedbackCommand command,
        CancellationToken cancellationToken = default
    )
    {
        command.CurrentUserId = CurrentUserId;
        return Ok(await _mediatr.Send(command, cancellationToken));
    }

    [Authorize]
    [HttpPatch("subscribe/{id:guid}")]
    public async Task<IActionResult> SubscribeOnProductAsync
    (
        [FromRoute] Guid id,
        CancellationToken cancellationToken = default
    ) => Ok(await _mediatr.Send(new SubscribeOnProductCommand(id, CurrentUserId), cancellationToken));
}
