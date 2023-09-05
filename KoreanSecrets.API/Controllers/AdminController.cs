using KoreanSecrets.BL.Behaviors.Admin.Brands.AddBrand;
using KoreanSecrets.BL.Behaviors.Admin.Categories.AddCategory;
using KoreanSecrets.BL.Behaviors.Admin.Countries.AddCountry;
using KoreanSecrets.BL.Behaviors.Admin.Demands.AddDemand;
using KoreanSecrets.BL.Behaviors.Admin.Products.AddProduct;
using KoreanSecrets.BL.Behaviors.Admin.Products.ChangeIsInStockStatus;
using KoreanSecrets.BL.Behaviors.Admin.SubCategories.AddSubCategory;
using KoreanSecrets.Domain.Common.Constants;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KoreanSecrets.API.Controllers;

[Authorize(Policy = AuthPolicies.Admins)]
[Route("api/admin")]
[ApiController]
public class AdminController : ControllerBase
{
    private readonly IMediator _mediatr;

    public AdminController(IMediator mediatr)
    {
        _mediatr = mediatr;
    }

    [HttpPost("brands/add")]
    public async Task<IActionResult> AddBrandAsync
    (
        [FromBody] AddBrandCommand command,
        CancellationToken cancellationToken = default
    ) => Ok(await _mediatr.Send(command, cancellationToken));

    [HttpPost("categories/add")]
    public async Task<IActionResult> AddCategoryAsync
    (
        [FromBody] AddCategoryCommand command,
        CancellationToken cancellationToken = default
    ) => Ok(await _mediatr.Send(command, cancellationToken));

    [HttpPost("countries/add")]
    public async Task<IActionResult> AddCountryAsync
    (
        [FromBody] AddCountryCommand command,
        CancellationToken cancellationToken = default
    ) => Ok(await _mediatr.Send(command, cancellationToken));

    [HttpPost("demands/add")]
    public async Task<IActionResult> AddDemandAsync
    (
        [FromBody] AddDemandCommand command,
        CancellationToken cancellationToken = default
    ) => Ok(await _mediatr.Send(command, cancellationToken));

    [HttpPost("products/add")]
    public async Task<IActionResult> AddProductAsync
    (
        [FromForm] AddProductCommand command,
        CancellationToken cancellationToken = default
    ) => Ok(await _mediatr.Send(command, cancellationToken));

    [HttpPost("subcategories/add")]
    public async Task<IActionResult> AddSubCategoryAsync
    (
        [FromBody] AddSubCategoryCommand command, 
        CancellationToken cancellationToken = default
    ) => Ok(await _mediatr.Send(command, cancellationToken));

    [HttpPut("products/{id:guid}/stock/toggle")]
    public async Task<IActionResult> ToggleIsInStockAsync
    (
        [FromRoute] Guid id,
        CancellationToken cancellationToken = default
    ) => Ok(await _mediatr.Send(new ChangeIsInStockStatusCommand(id), cancellationToken));
}
