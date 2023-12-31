﻿using KoreanSecrets.BL.Behaviors.Admin.Banners.AddBanner;
using KoreanSecrets.BL.Behaviors.Admin.Banners.ChangeBannerPhoto;
using KoreanSecrets.BL.Behaviors.Admin.Banners.ChangeBannerText;
using KoreanSecrets.BL.Behaviors.Admin.Banners.DeleteBanner;
using KoreanSecrets.BL.Behaviors.Admin.Brands.AddBrand;
using KoreanSecrets.BL.Behaviors.Admin.Brands.ChangeBrandPhoto;
using KoreanSecrets.BL.Behaviors.Admin.Brands.DeleteBrand;
using KoreanSecrets.BL.Behaviors.Admin.Brands.ModifyBrand;
using KoreanSecrets.BL.Behaviors.Admin.Categories.AddCategory;
using KoreanSecrets.BL.Behaviors.Admin.Categories.DeleteCategory;
using KoreanSecrets.BL.Behaviors.Admin.Categories.ModifyCategory;
using KoreanSecrets.BL.Behaviors.Admin.Countries.AddCountry;
using KoreanSecrets.BL.Behaviors.Admin.Countries.DeleteCountry;
using KoreanSecrets.BL.Behaviors.Admin.Countries.ModifyCountry;
using KoreanSecrets.BL.Behaviors.Admin.Demands.AddDemand;
using KoreanSecrets.BL.Behaviors.Admin.Demands.DeleteDemand;
using KoreanSecrets.BL.Behaviors.Admin.Demands.ModifyDemand;
using KoreanSecrets.BL.Behaviors.Admin.Products.AddDiscount;
using KoreanSecrets.BL.Behaviors.Admin.Products.AddGuide;
using KoreanSecrets.BL.Behaviors.Admin.Products.AddPhotoToList;
using KoreanSecrets.BL.Behaviors.Admin.Products.AddProduct;
using KoreanSecrets.BL.Behaviors.Admin.Products.ChangeIsInStockStatus;
using KoreanSecrets.BL.Behaviors.Admin.Products.ChangeMainPhoto;
using KoreanSecrets.BL.Behaviors.Admin.Products.DeleteGuide;
using KoreanSecrets.BL.Behaviors.Admin.Products.DeletePhotoFromList;
using KoreanSecrets.BL.Behaviors.Admin.Products.GetAllProducts;
using KoreanSecrets.BL.Behaviors.Admin.Products.ModifyProduct;
using KoreanSecrets.BL.Behaviors.Admin.Products.RemoveDiscount;
using KoreanSecrets.BL.Behaviors.Admin.Promocodes.AddPromocode;
using KoreanSecrets.BL.Behaviors.Admin.Promocodes.DeletePromocode;
using KoreanSecrets.BL.Behaviors.Admin.SubCategories.AddSubCategory;
using KoreanSecrets.BL.Behaviors.Admin.SubCategories.DeleteSubCategory;
using KoreanSecrets.BL.Behaviors.Admin.SubCategories.ModifySubCategory;
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

    [HttpPost("products/all")]
    public async Task<IActionResult> GetProductsAsync
    (
        [FromBody] GetAllProductsQuery command,
        CancellationToken cancellationToken = default
    ) => Ok(await _mediatr.Send(command, cancellationToken));

    [HttpPost("brands/add")]
    public async Task<IActionResult> AddBrandAsync
    (
        [FromForm] AddBrandCommand command,
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

    [HttpPatch("products/{id:guid}/stock/toggle")]
    public async Task<IActionResult> ToggleIsInStockAsync
    (
        [FromRoute] Guid id,
        CancellationToken cancellationToken = default
    ) => Ok(await _mediatr.Send(new ChangeIsInStockStatusCommand(id), cancellationToken));

    [HttpPatch("products/{id:guid}/discount/add")]
    public async Task<IActionResult> AddDiscountToProductAsync
    (
        [FromRoute] Guid id,
        [FromQuery] long newPrice,
        CancellationToken cancellationToken = default
    ) => Ok(await _mediatr.Send(new AddDiscountCommand(id, newPrice), cancellationToken));

    [HttpPatch("products/{id:guid}/discount/remove")]
    public async Task<IActionResult> RemoveDiscountFromProductAsync
    (
        [FromRoute] Guid id,
        CancellationToken cancellationToken = default
    ) => Ok(await _mediatr.Send(new RemoveDiscountCommand(id), cancellationToken));

    [HttpPatch("products/photos/main/change")]
    public async Task<IActionResult> ChangeMainPhotoAsync
    (
        [FromForm] ChangeMainPhotoCommand command,
        CancellationToken cancellationToken = default
    ) => Ok(await _mediatr.Send(command, cancellationToken));

    [HttpPatch("products/photos/add")]
    public async Task<IActionResult> AddPhotoAsync
    (
        [FromForm] AddPhotoToListCommand command,
        CancellationToken cancellationToken = default
    ) => Ok(await _mediatr.Send(command, cancellationToken));

    [HttpPatch("products/photos/{id:guid}/remove")]
    public async Task<IActionResult> RemovePhotoAsync
    (
        [FromRoute] Guid id,
        CancellationToken cancellationToken = default
    ) => Ok(await _mediatr.Send(new DeletePhotoFromListCommand(id), cancellationToken));

    [HttpPatch("products/guide/add")]
    public async Task<IActionResult> AddGuideAsync
    (
        [FromForm] AddGuideCommand command,
        CancellationToken cancellationToken = default
    ) => Ok(await _mediatr.Send(command, cancellationToken));

    [HttpPatch("products/{productId:guid}/guide/{guideId:guid}/remove")]
    public async Task<IActionResult> RemoveGuideAsync
    (
        [FromRoute] Guid productId,
        [FromRoute] Guid guideId,
        CancellationToken cancellationToken = default
    ) => Ok(await _mediatr.Send(new DeleteGuideCommand(productId, guideId), cancellationToken));

    [HttpPut("product/{id:guid}/modify")]
    public async Task<IActionResult> ModifyProductAsync
    (
        [FromRoute] Guid id,
        [FromBody] ModifyProductCommand command,
        CancellationToken cancellationToken = default
    )
    {
        command.ProductId = id;

        return Ok(await _mediatr.Send(command, cancellationToken));
    }

    [HttpPatch("brand/photo/change")]
    public async Task<IActionResult> ChangeBrandPhotoAsync
    (
        [FromForm] ChangeBrandPhotoCommand command,
        CancellationToken cancellationToken = default
    ) => Ok(await _mediatr.Send(command, cancellationToken));

    [HttpPatch("brand/{id:guid}/modify")]
    public async Task<IActionResult> ModifyBrandAsync
    (
        [FromRoute] Guid id,
        [FromBody] ModifyBrandCommand command,
        CancellationToken cancellationToken = default
    )
    {
        command.Id = id;

        return Ok(await _mediatr.Send(command, cancellationToken));
    }

    [HttpPatch("category/{id:guid}/modify")]
    public async Task<IActionResult> ModifyCategoryAsync
    (
        [FromRoute] Guid id,
        [FromBody] ModifyCategoryCommand command,
        CancellationToken cancellationToken = default
    )
    {
        command.Id = id;

        return Ok(await _mediatr.Send(command, cancellationToken));
    }

    [HttpPatch("country/{id:guid}/modify")]
    public async Task<IActionResult> ModifyCountryAsync
    (
        [FromRoute] Guid id,
        [FromBody] ModifyCountryCommand command,
        CancellationToken cancellationToken = default
    )
    {
        command.Id = id;

        return Ok(await _mediatr.Send(command, cancellationToken));
    }

    [HttpPatch("demand/{id:guid}/modify")]
    public async Task<IActionResult> ModifyDemandAsync
    (
        [FromRoute] Guid id,
        [FromBody] ModifyDemandCommand command,
        CancellationToken cancellationToken = default
    )
    {
        command.Id = id;

        return Ok(await _mediatr.Send(command, cancellationToken));
    }

    [HttpPatch("subсategory/{id:guid}/modify")]
    public async Task<IActionResult> ModifySubCategoryAsync
    (
        [FromRoute] Guid id,
        [FromBody] ModifySubCategoryCommand command,
        CancellationToken cancellationToken = default
    )
    {
        command.Id = id;

        return Ok(await _mediatr.Send(command, cancellationToken));
    }

    [HttpDelete("brand/{id:guid}")]
    public async Task<IActionResult> DeleteBrandAsync
    (
        [FromRoute] Guid id,
        CancellationToken cancellationToken = default
    ) => Ok(await _mediatr.Send(new DeleteBrandCommand(id), cancellationToken));

    [HttpDelete("categories/{id:guid}")]
    public async Task<IActionResult> DeleteCategoryAsync
    (
        [FromRoute] Guid id,
        CancellationToken cancellationToken = default
    ) => Ok(await _mediatr.Send(new DeleteCategoryCommand(id), cancellationToken));

    [HttpDelete("country/{id:guid}")]
    public async Task<IActionResult> DeleteCountryAsync
    (
        [FromRoute] Guid id,
        CancellationToken cancellationToken = default
    ) => Ok(await _mediatr.Send(new DeleteCountryCommand(id), cancellationToken));

    [HttpDelete("demands/{id:guid}")]
    public async Task<IActionResult> DeleteDemandAsync
    (
        [FromRoute] Guid id,
        CancellationToken cancellationToken = default
    ) => Ok(await _mediatr.Send(new DeleteDemandCommand(id), cancellationToken));

    [HttpDelete("subcategory/{id:guid}")]
    public async Task<IActionResult> DeleteSubCategoryAsync
    (
        [FromRoute] Guid id,
        CancellationToken cancellationToken = default
    ) => Ok(await _mediatr.Send(new DeleteSubCategoryCommand(id), cancellationToken));

    [HttpPost("promocodes/add")]
    public async Task<IActionResult> AddPromocodeAsync
    (
        [FromBody] AddPromocodeCommand command,
        CancellationToken cancellationToken = default
    ) => Ok(await _mediatr.Send(command, cancellationToken));

    [HttpDelete("promocodes/{id:guid}/delete")]
    public async Task<IActionResult> DeletePromocodeAsync
    (
        [FromRoute] Guid id,
        CancellationToken cancellationToken = default
    ) => Ok(await _mediatr.Send(new DeletePromocodeCommand(id), cancellationToken));

    [HttpPost("banners/add")]
    public async Task<IActionResult> AddBannerAsync
    (
        [FromForm] AddBannerCommand command,
        CancellationToken cancellationToken = default
    ) => Ok(await _mediatr.Send(command, cancellationToken));

    [HttpPatch("banners/photo/change")]
    public async Task<IActionResult> ChangeBannerPhotoAsync
    (
        [FromForm] ChangeBannerPhotoCommand command,
        CancellationToken cancellationToken = default
    ) => Ok(await _mediatr.Send(command, cancellationToken));

    [HttpPatch("banners/modify")]
    public async Task<IActionResult> ModifyBannerAsync
    (
        [FromBody] ChangeBannerTextCommand command,
        CancellationToken cancellationToken = default
    ) => Ok(await _mediatr.Send(command, cancellationToken));

    [HttpDelete("banners/{id:guid}/delete")]
    public async Task<IActionResult> DeleteBannerAsync
    (
        [FromRoute] Guid id,
        CancellationToken cancellationToken = default
    ) => Ok(await _mediatr.Send(new DeleteBannerCommand(id), cancellationToken));
}
