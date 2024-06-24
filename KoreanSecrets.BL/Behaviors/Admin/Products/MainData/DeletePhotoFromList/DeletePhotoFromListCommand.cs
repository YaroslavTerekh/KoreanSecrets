using System.Text.Json.Serialization;
using MediatR;

namespace KoreanSecrets.BL.Behaviors.Admin.Products.MainData.DeletePhotoFromList;

public class DeletePhotoFromListCommand : IRequest
{
    [JsonIgnore]
    public Guid PhotoId { get; set; }

    public DeletePhotoFromListCommand(Guid id) => PhotoId = id;
}
