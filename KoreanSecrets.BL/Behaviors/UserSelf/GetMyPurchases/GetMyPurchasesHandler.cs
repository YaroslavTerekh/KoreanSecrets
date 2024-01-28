using KoreanSecrets.Domain.DataTransferObjects;
using KoreanSecrets.Domain.DbConnection;
using KoreanSecrets.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoreanSecrets.BL.Behaviors.UserSelf.GetMyPurchases;

public class GetMyPurchasesHandler : IRequestHandler<GetMyPurchasesQuery, PaginationModelDTO<Purchase>>
{
    private readonly DataContext _context;

    public GetMyPurchasesHandler(DataContext context)
    {
        _context = context;
    }

    public async Task<PaginationModelDTO<Purchase>> Handle(GetMyPurchasesQuery request, CancellationToken cancellationToken)
    {
        var userPurchases = _context.Purchases.Where(t => t.UserId == request.CurrentUserId);

        var result = await userPurchases.Skip(request.PageSize * request.CurrentPage)
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);


        return new PaginationModelDTO<Purchase>()
        {
            CurrentPage = request.CurrentPage,
            PageSize = request.PageSize,
            Products = result,
            Total = await userPurchases.CountAsync(cancellationToken)
        };
    }
}
