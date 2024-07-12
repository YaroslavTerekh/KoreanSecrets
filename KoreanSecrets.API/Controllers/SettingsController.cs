using KoreanSecrets.BL.Behaviors.Settings.EnableLiqPay;
using KoreanSecrets.BL.Behaviors.Settings.EnablePayByCard;
using KoreanSecrets.BL.Behaviors.Settings.GetSettings;
using KoreanSecrets.Domain.Common.Constants;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KoreanSecrets.API.Controllers;

[Route("api/settings")]
[ApiController]
public class SettingController : ControllerBase
{
    private readonly IMediator _mediatr;

    public SettingController(IMediator mediatr)
    {
        _mediatr = mediatr;
    }

    [Authorize(Policy = AuthPolicies.Admins)]
    [HttpGet("enable-liq-pay/toggle")]
    public async Task<IActionResult> EnableLiqPayAsync
    (
        CancellationToken cancellationToken = default
    ) => Ok(await _mediatr.Send(new EnableLiqPayCommand(), cancellationToken));
    
    [Authorize(Policy = AuthPolicies.Admins)]
    [HttpGet("enable-card-pay/toggle")]
    public async Task<IActionResult> EnablePayByCardAsync
    (
        CancellationToken cancellationToken = default
    ) => Ok(await _mediatr.Send(new EnablePayByCardCommand(), cancellationToken));
    
    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetSettingsAsync
    (
        CancellationToken cancellationToken = default
    ) => Ok(await _mediatr.Send(new GetSettingsCommand(), cancellationToken));
}
