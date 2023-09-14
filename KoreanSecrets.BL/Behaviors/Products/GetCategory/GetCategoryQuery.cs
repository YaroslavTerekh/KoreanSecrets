using KoreanSecrets.Domain.DataTransferObjects;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoreanSecrets.BL.Behaviors.Products.GetCategory;

public class GetCategoryQuery : IRequest<CategoryDTO>
{
    public Guid Id { get; set; }

    public int PageSize { get; set; }

    public int CurrentPage { get; set; }
}
