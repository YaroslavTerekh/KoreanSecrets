using MediatR;

namespace KoreanSecrets.BL.Behaviors.Admin.Products.Volumes.ModifyVolumeQuantity;

public class ModifyVolumeQuantityCommand : IRequest
{
    public Guid Id { get; set; }
    public int Quantity { get; set; }
}
