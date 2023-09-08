using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoreanSecrets.BL.Behaviors.Admin.Brands.ChangeBrandPhoto;

public class ChangeBrandPhotoCommand : IRequest
{
    public Guid BrandId { get; set; }

    public IFormFile Photo { get; set; }
}
