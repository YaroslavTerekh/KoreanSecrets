using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoreanSecrets.BL.Behaviors.Admin.Banners.ChangeBannerPhoto;

public class ChangeBannerPhotoCommand : IRequest
{
    public Guid BannerId { get; set; }

    public IFormFile Photo { get; set; }
}
