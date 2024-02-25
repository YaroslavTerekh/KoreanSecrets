using KoreanSecrets.Domain.DataTransferObjects;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoreanSecrets.BL.Behaviors.Admin.Products.GetProducts;

public class GetAdminProductsQuery : IRequest<PaginationModelDTO<PageProductDTO>>
{
    public string SearchText { get; set; }

    public int CurrentPage { get; set; }

    public int PageSize { get; set; }
}
