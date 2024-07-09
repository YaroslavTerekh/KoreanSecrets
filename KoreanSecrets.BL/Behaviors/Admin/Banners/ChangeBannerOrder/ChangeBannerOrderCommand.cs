using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoreanSecrets.BL.Behaviors.Admin.Banners.ChangeBannerOrder;

public class ChangeBannerOrderCommand : IRequest
{
    public Guid BannerId { get; set; }

    public int Order { get; set; }
}
