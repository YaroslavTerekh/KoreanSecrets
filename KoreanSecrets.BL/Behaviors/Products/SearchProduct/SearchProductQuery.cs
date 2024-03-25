using KoreanSecrets.Domain.DataTransferObjects;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoreanSecrets.BL.Behaviors.Products.SearchProduct;

public class SearchProductQuery : IRequest<List<ListProductDTO>>
{
    public string SearchText { get; set; }
}
