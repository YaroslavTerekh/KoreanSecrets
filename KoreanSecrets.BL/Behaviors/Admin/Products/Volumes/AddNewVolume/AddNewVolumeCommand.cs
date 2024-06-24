using MediatR;
using Microsoft.AspNetCore.Http;

namespace KoreanSecrets.BL.Behaviors.Admin.Products.Volumes.AddNewVolume;

public class AddNewVolumeCommand : IRequest
{
    public Guid ProductId { get; set; }

    public string Value { get; set; }

    public string Unit { get; set; }

    public long Price { get; set; }

    public int Quantity { get; set; }

    public List<IFormFile>? Photos { get; set; }
}
