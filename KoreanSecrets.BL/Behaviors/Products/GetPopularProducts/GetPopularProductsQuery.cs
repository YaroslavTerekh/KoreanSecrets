using KoreanSecrets.Domain.DataTransferObjects;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoreanSecrets.BL.Behaviors.Products.GetPopularProducts;

public class GetPopularProductsQuery : IRequest<List<ListProductDTO>>
{
    public int PageSize { get; set; }
    
    public int CurrentPage { get; set; }
}
