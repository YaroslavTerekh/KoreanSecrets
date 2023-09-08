using KoreanSecrets.Domain.DataTransferObjects;
using KoreanSecrets.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoreanSecrets.BL.Behaviors.Products.GetProducts;

public class GetProductsQuery : IAuthorizedRequest<PaginationModelDTO<ListProductDTO>>
{
    public Guid CategoryId { get; set; }

    public List<Guid> BrandsIds { get; set; } = new();

    public List<Guid> DemandsIds { get; set; } = new();

    public List<Guid> CountriesIds { get; set; } = new();

    public List<Guid> SubCategoriesIds { get; set; } = new();

    public int CurrentPage { get; set; }

    public int PageSize { get; set; }
}
