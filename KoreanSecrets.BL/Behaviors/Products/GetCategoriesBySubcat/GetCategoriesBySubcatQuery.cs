using KoreanSecrets.Domain.DataTransferObjects;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoreanSecrets.BL.Behaviors.Products.GetCategoriesBySubcat;

public class GetCategoriesBySubcatQuery : IRequest<List<CategoryDTO>>
{
    public Guid SubcategoryId { get; set; }

    public GetCategoriesBySubcatQuery(Guid id) => SubcategoryId = id;
}
