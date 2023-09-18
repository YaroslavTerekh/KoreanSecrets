using KoreanSecrets.Domain.DataTransferObjects;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoreanSecrets.BL.Behaviors.Products.GetDiscountedProducts;

public class GetDiscountedProductsQuery : IAuthorizedRequest<PaginationModelDTO<ListProductDTO>>
{
    public int CurrentPage { get; set; }

    public int PageSize { get; set; }
}
