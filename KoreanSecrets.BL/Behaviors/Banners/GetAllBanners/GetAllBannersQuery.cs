using KoreanSecrets.Domain.DataTransferObjects;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KoreanSecrets.Domain.DataTransferObjects.Banners;

namespace KoreanSecrets.BL.Behaviors.Banners.GetAllBanners;

public class GetAllBannersQuery : IRequest<List<BannerDTO>>
{
}
