﻿using KoreanSecrets.Domain.DataTransferObjects;
using KoreanSecrets.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoreanSecrets.BL.Behaviors.Admin.Common.GetSubCatsWithNullCategory;

public class GetSubCatWithNullCategoryQuery : IAuthorizedRequest<AdminPaginationNullProperties<SubCategory>>
{
    public int CurrentPage { get; set; }

    public int PageSize { get; set; }

    public bool Desc { get; set; }
}
