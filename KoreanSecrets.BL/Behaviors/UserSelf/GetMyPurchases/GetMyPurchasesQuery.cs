using KoreanSecrets.Domain.DataTransferObjects;
using KoreanSecrets.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoreanSecrets.BL.Behaviors.UserSelf.GetMyPurchases;

public class GetMyPurchasesQuery : IAuthorizedRequest<PaginationModelDTO<Purchase>>
{
    public int CurrentPage { get; set; }
    
    public int PageSize { get; set; }
}
