using KoreanSecrets.Domain.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoreanSecrets.BL.Behaviors.UserSelf.GetLikedProducts;

public class GetLikedProductsQuery : IAuthorizedRequest<PaginationModelDTO<ListProductDTO>>
{
    public int CurrentPage { get; set; }

    public int PageSize { get; set; }
}
