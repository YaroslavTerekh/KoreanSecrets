using MediatR;

namespace KoreanSecrets.BL.Behaviors.Admin.Products.Volumes.ModifyVolume;

public class ModifyVolumeCommand : IRequest
{
    public Guid Id { get; set; }

    public string Value { get; set; }

    public string Unit { get; set; }

    public int Quantity { get; set; }

    public decimal Price { get; set; }
}
