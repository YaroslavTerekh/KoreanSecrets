using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoreanSecrets.BL.Behaviors.Admin.Banners.DeleteBanner;

public class DeleteBannerCommand : IRequest
{
    public Guid BannerId { get; set; }

    public DeleteBannerCommand(Guid id) => BannerId = id;
}
