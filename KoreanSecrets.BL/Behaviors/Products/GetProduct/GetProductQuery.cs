using KoreanSecrets.Domain.DataTransferObjects;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoreanSecrets.BL.Behaviors.Products.GetProduct;

public class GetProductQuery : IRequest<PageProductDTO>
{
    public Guid ProductId { get; set; }

    public GetProductQuery(Guid id) => ProductId = id;
}
