using MediatR;
using Microsoft.AspNetCore.Http;

namespace KoreanSecrets.BL.Behaviors.Admin.Products.MainData.AddPhotoToList;

public class AddPhotoToListCommand : IRequest
{
    public Guid ProductId { get; set; }

    public IFormFile Photo { get; set; }
}
