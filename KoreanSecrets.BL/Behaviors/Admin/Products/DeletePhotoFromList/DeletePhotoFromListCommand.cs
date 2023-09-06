using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace KoreanSecrets.BL.Behaviors.Admin.Products.DeletePhotoFromList;

public class DeletePhotoFromListCommand : IRequest
{
    [JsonIgnore]
    public Guid PhotoId { get; set; }

    public DeletePhotoFromListCommand(Guid id) => PhotoId = id;
}
