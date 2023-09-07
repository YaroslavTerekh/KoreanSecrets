using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace KoreanSecrets.BL.Behaviors.Admin.Products.AddGuide;

public class AddGuideCommand : IRequest
{
    public Guid ProductId { get; set; }

    public IFormFile File { get; set; }
}
