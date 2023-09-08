﻿using KoreanSecrets.BL.Behaviors.Products.DislikeProduct;
using KoreanSecrets.BL.Behaviors.Products.GetBrands;
using KoreanSecrets.BL.Behaviors.Products.GetDiscountedProducts;
using KoreanSecrets.BL.Behaviors.Products.GetProduct;
using KoreanSecrets.BL.Behaviors.Products.GetProducts;
using KoreanSecrets.BL.Behaviors.Products.LikeProduct;
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
}
