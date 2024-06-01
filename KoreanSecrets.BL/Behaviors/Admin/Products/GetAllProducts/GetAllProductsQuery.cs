using KoreanSecrets.Domain.DataTransferObjects;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoreanSecrets.BL.Behaviors.Admin.Products.GetAllProducts;

public class GetAllProductsQuery : IRequest<PaginationModelDTO<PageProductDTO>>
{
    public int CurrentPage { get; set; }

    public int PageSize { get; set; }
    
    public string? Text { get; set; }
    
    public string? WayToSort { get; set; }
    
    public string? ColumnToSort { get; set; }
}
