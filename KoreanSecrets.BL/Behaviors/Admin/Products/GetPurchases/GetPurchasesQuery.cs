using KoreanSecrets.Domain.Common.Enums;
using KoreanSecrets.Domain.DataTransferObjects;
using KoreanSecrets.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoreanSecrets.BL.Behaviors.Admin.Products.GetPurchases;

public class GetPurchasesQuery : IRequest<PaginationModelDTO<Purchase>>
{
    public int CurrentPage { get; set; }

    public int PageSize { get; set; }

    public PurchaseStatus? Status { get; set; }
}
