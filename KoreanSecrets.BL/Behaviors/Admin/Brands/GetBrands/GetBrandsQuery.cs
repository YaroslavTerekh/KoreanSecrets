﻿using KoreanSecrets.Domain.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoreanSecrets.BL.Behaviors.Admin.Brands.GetBrands;

public class GetBrandsQuery : IAuthorizedRequest<PaginationModelDTO<BrandDTO>>
{
    public int CurrentPage { get; set; }

    public int PageSize { get; set; }

    public bool Desc { get; set; }
}