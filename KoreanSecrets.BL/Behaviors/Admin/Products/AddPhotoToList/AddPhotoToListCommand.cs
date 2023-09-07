using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoreanSecrets.BL.Behaviors.Admin.Products.AddPhotoToList;

public class AddPhotoToListCommand : IRequest
{
    public Guid ProductId { get; set; }

    public IFormFile Photo { get; set; }
}
