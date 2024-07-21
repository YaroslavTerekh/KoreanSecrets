using MediatR;

namespace KoreanSecrets.BL.Behaviors.Admin.Promocodes.DeletePromocodeProduct;

public class DeletePromocodeProductCommand : IRequest
{
    public Guid PromocodeId { get; set; }

    public Guid ProductId { get; set; }
}
