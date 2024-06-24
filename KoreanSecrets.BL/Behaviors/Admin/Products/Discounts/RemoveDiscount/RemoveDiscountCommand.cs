using MediatR;

namespace KoreanSecrets.BL.Behaviors.Admin.Products.Discounts.RemoveDiscount;

public class RemoveDiscountCommand : IRequest
{
    public Guid ProductId { get; set; }

    public RemoveDiscountCommand(Guid id) => ProductId = id;
}
