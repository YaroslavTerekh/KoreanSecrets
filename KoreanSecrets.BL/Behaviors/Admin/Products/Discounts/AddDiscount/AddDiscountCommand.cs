using MediatR;

namespace KoreanSecrets.BL.Behaviors.Admin.Products.Discounts.AddDiscount;

public class AddDiscountCommand : IRequest
{
    public Guid ProductId { get; set; }

    public decimal NewPrice { get; set; }

    public DateTime DiscountPriceStartDate { get; set; }

    public DateTime DiscountPriceEndDate { get; set; }
}
