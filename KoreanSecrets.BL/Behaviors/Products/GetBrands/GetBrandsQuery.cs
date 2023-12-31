﻿using KoreanSecrets.Domain.DataTransferObjects;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoreanSecrets.BL.Behaviors.Products.GetBrands;

public class GetBrandsQuery : IRequest<PaginationModelDTO<BrandDTO>>
{
    public int CurrentPage { get; set; }

    public int PageSize { get; set; }
}
