using MediatR;

namespace KoreanSecrets.BL.Behaviors.Admin.Products.DeleteProduct;

public class DeleteProductCommand : IRequest
{
    public Guid ProductId { get; set; }

    public DeleteProductCommand(Guid productId)
    {
        ProductId = productId;
    }
}
