using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoreanSecrets.BL.Behaviors.Admin.Banners.AddBanner;

public class AddBannerCommand : IRequest
{
    public string Title { get; set; }

    public IFormFile Photo { get; set; }

    public Guid ProductId { get; set; }
}
