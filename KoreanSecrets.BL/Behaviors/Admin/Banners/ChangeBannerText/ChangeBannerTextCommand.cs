using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoreanSecrets.BL.Behaviors.Admin.Banners.ChangeBannerText;

public class ChangeBannerTextCommand : IRequest
{
    public Guid BannerId { get; set; }

    public string Title { get; set; }
}
