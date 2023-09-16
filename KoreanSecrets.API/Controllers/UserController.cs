using KoreanSecrets.BL.Behaviors.UserSelf.AddFeedback;
using KoreanSecrets.BL.Behaviors.UserSelf.AddProductToBucket;
using KoreanSecrets.BL.Behaviors.UserSelf.GetBucket;
using KoreanSecrets.BL.Behaviors.UserSelf.ModifyAddressInfo;
using KoreanSecrets.BL.Behaviors.UserSelf.RemoveProductFromBucket;
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

    [Authorize]
    [HttpPatch("bucket/add/{id:guid}")]
    public async Task<IActionResult> AddProductToBucketAsync
    (
        [FromRoute] Guid id,
        CancellationToken cancellationToken = default
    ) => Ok(await _mediatr.Send(new AddProductToBucketCommand(id, CurrentUserId), cancellationToken));

    [Authorize]
    [HttpPatch("bucket/remove/{id:guid}")]
    public async Task<IActionResult> RemoveProductFromBucketAsync
    (
        [FromRoute] Guid id,
        CancellationToken cancellationToken = default
    ) => Ok(await _mediatr.Send(new RemoveProductFromBucketCommand(id, CurrentUserId), cancellationToken));

    [Authorize]
    [HttpPatch("bucket/get")]
    public async Task<IActionResult> GetBucketAsync
    (
        CancellationToken cancellationToken = default
    ) => Ok(await _mediatr.Send(new GetBucketQuery(CurrentUserId), cancellationToken));

    [Authorize]
    [HttpPut("address/modify")]
    public async Task<IActionResult> ModifyAddressInfoAsync
    (
        [FromBody] ModifyAddressInfoCommand command,
        CancellationToken cancellationToken = default
    )
    {
        command.CurrentUserId = CurrentUserId;

        return Ok(await _mediatr.Send(command, cancellationToken));
    }
}
