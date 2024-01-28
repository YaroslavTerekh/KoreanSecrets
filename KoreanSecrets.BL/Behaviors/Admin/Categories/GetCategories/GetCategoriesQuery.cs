using KoreanSecrets.Domain.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoreanSecrets.BL.Behaviors.Admin.Categories.GetCategories;

public class GetCategoriesQuery : IAuthorizedRequest<PaginationModelDTO<CategoryDTO>>
{
    public int CurrentPage { get; set; }

    public int PageSize { get; set; }

    public bool Desc { get; set; }
}
