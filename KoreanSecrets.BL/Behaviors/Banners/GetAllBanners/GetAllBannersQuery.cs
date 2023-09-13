using KoreanSecrets.Domain.DataTransferObjects;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoreanSecrets.BL.Behaviors.Banners.GetAllBanners;

public class GetAllBannersQuery : IRequest<List<BannerDTO>>
{
}
