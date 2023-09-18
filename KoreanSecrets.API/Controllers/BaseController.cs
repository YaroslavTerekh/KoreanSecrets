using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace KoreanSecrets.API.Controllers;

public class BaseController : ControllerBase
{
    protected Guid CurrentUserId => !User.Identity.IsAuthenticated ? Guid.Empty : Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
}
