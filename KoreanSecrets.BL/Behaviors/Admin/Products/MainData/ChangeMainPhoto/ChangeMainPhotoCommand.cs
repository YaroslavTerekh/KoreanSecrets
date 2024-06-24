using System.Text.Json.Serialization;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace KoreanSecrets.BL.Behaviors.Admin.Products.MainData.ChangeMainPhoto;

public class ChangeMainPhotoCommand : IRequest
{
    [JsonIgnore]
    public Guid ProductId { get; set; }

    public IFormFile MainPhoto { get; set; }
}
