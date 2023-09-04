using KoreanSecrets.BL.Behaviors.Products.GetProduct;
using KoreanSecrets.BL.Behaviors.Products.GetProducts;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KoreanSecrets.API.Controllers;

[Route("api/products")]
[ApiController]
public class ProductsController : ControllerBase
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
    ) => Ok(await _mediatr.Send(query, cancellationToken));

    [HttpPost("get/{id:guid}")]
    public async Task<IActionResult> GetProductAsync
    (
        [FromRoute] Guid id,
        CancellationToken cancellationToken = default
    ) => Ok(await _mediatr.Send(new GetProductQuery(id), cancellationToken));
}
