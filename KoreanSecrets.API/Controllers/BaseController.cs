using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace KoreanSecrets.API.Controllers;

public class BaseController : ControllerBase
{
    protected Guid CurrentUserId => Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
}
