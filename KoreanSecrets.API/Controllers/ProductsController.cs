using KoreanSecrets.BL.Behaviors.Banners.GetAllBanners;
using KoreanSecrets.BL.Behaviors.NovaPost.GetAllCities;
using KoreanSecrets.BL.Behaviors.NovaPost.GetWarehouses;
using KoreanSecrets.BL.Behaviors.Products.DislikeProduct;
using KoreanSecrets.BL.Behaviors.Products.GetBrands;
using KoreanSecrets.BL.Behaviors.Products.GetCategories;
using KoreanSecrets.BL.Behaviors.Products.GetCategory;
using KoreanSecrets.BL.Behaviors.Products.GetDiscountedProducts;
using KoreanSecrets.BL.Behaviors.Products.GetPopularProducts;
using KoreanSecrets.BL.Behaviors.Products.GetProduct;
using KoreanSecrets.BL.Behaviors.Products.GetProducts;
using KoreanSecrets.BL.Behaviors.Products.LikeProduct;
using KoreanSecrets.BL.Behaviors.Purchases.GeneratePurchase;
using KoreanSecrets.BL.Behaviors.UserSelf.GetLikedProducts;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KoreanSecrets.API.Controllers;

[Route("api/products")]
[ApiController]
public class ProductsController : BaseController
{
    private readonly IMediator _mediatr;

    public ProductsController(IMediator mediator)
    {
        _mediatr = mediator;
    }

    [HttpPost("get")]
    public async Task<IActionResult> GetProductsAsync
    (
        [FromBody] GetProductsQuery query,
        CancellationToken cancellationToken = default
    )
    {
        query.CurrentUserId = CurrentUserId;

        return Ok(await _mediatr.Send(query, cancellationToken)); 
    }

    [HttpPost("get/{id:guid}")]
    public async Task<IActionResult> GetProductAsync
    (
        [FromRoute] Guid id,
        CancellationToken cancellationToken = default
    )
    {
        var query = new GetProductQuery(id);
        query.CurrentUserId = CurrentUserId;

        return Ok(await _mediatr.Send(query, cancellationToken));
    }

    [HttpPost("get/discounted")]
    public async Task<IActionResult> GetDiscountedAsync
    (
        [FromBody] GetDiscountedProductsQuery query,
        CancellationToken cancellationToken = default
    )
    {
        query.CurrentUserId = CurrentUserId;

        return Ok(await _mediatr.Send(query, cancellationToken));
    }

    [Authorize]
    [HttpPost("get/liked")]
    public async Task<IActionResult> GetLikedAsync
    (
        [FromBody] GetLikedProductsQuery query,
        CancellationToken cancellationToken = default
    )
    {
        query.CurrentUserId = CurrentUserId;

        return Ok(await _mediatr.Send(query, cancellationToken));
    }

    [Authorize]
    [HttpPatch("like/{id:guid}")]
    public async Task<IActionResult> LikeProductAsync
    (
        [FromRoute] Guid id,
        CancellationToken cancellationToken = default
    )
    {
        return Ok(await _mediatr.Send(new LikeProductCommand(id, CurrentUserId), cancellationToken));
    }

    [Authorize]
    [HttpPatch("unlike/{id:guid}")]
    public async Task<IActionResult> UnlikeProductAsync
    (
        [FromRoute] Guid id,
        CancellationToken cancellationToken = default
    )
    {
        return Ok(await _mediatr.Send(new DislikeProductCommand(id, CurrentUserId), cancellationToken));
    }

    [HttpPost("brands/get")]
    public async Task<IActionResult> GetBrandsAsync
    (
        [FromBody] GetBrandsQuery query,
        CancellationToken cancellationToken = default
    ) => Ok(await _mediatr.Send(query, cancellationToken));

    [HttpGet("banners")]
    public async Task<IActionResult> GetBannersAsync
    (CancellationToken cancellationToken = default)
        => Ok(await _mediatr.Send(new GetAllBannersQuery(), cancellationToken));

    [HttpGet("categories")]
    public async Task<IActionResult> GetCategoriesAsync
    (CancellationToken cancellationToken = default)
        => Ok(await _mediatr.Send(new GetCategoriesQuery(), cancellationToken));

    [HttpPost("category/get")]
    public async Task<IActionResult> GetCategoryAsync
    (
        [FromBody] GetCategoryQuery query,
        CancellationToken cancellationToken = default
    ) => Ok(await _mediatr.Send(query, cancellationToken));

    [HttpPost("get/popular")]
    public async Task<IActionResult> GetPopularProductsAsync
    (
        [FromBody] GetPopularProductsQuery query,
        CancellationToken cancellationToken = default
    ) => Ok(await _mediatr.Send(query, cancellationToken));

    [HttpGet("all/cities")]
    public async Task<IActionResult> GetAllCitiesAsync
    (
        [FromQuery] string data
    ) => Ok(await _mediatr.Send(new GetAllCitiesQuery(data)));

    [HttpGet("all/cities/warehouses")]
    public async Task<IActionResult> GetWarehousesAsync
    (
        [FromQuery] string data,
        [FromQuery] string warehouse
    ) => Ok(await _mediatr.Send(new GetWarehousesQuery(data, warehouse)));

    [Authorize]
    [HttpPost("purchase/generate")]
    public async Task<IActionResult> GeneratePurchaseAsync
    (
        [FromBody] GeneratePurchaseCommand command,
        CancellationToken cancellationToken = default
    )
    {
        command.CurrentUserId = CurrentUserId;

        return Ok(await _mediatr.Send(command, cancellationToken));
    }
}
