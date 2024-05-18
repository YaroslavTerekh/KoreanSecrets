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
using KoreanSecrets.Domain.Common.Enums;

namespace KoreanSecrets.BL.Behaviors.Admin.Products.GetPurchases;

public class GetPurchasesHandler : IRequestHandler<GetPurchasesQuery, PaginationModelDTO<Purchase>>
{
    private readonly DataContext _context;

    public GetPurchasesHandler(DataContext context)
    {
        _context = context;
    }

    public async Task<PaginationModelDTO<Purchase>> Handle(GetPurchasesQuery request, CancellationToken cancellationToken)
    {
        var purchases = _context.Purchases
            .Include(t => t.Products)
            .OrderBy(t => t.CreatedDate)
            .AsQueryable();

        var total = await purchases.CountAsync(cancellationToken);

        if(request.Status is not null)
        {
            if (request.Status == PurchaseStatus.New || request.Status == PurchaseStatus.Waiting)
            {
                purchases = purchases.Where(t => (t.PurchaseStatus == PurchaseStatus.New || t.PurchaseStatus == PurchaseStatus.Waiting));
            }
            
            if (request.Status == PurchaseStatus.Success || request.Status == PurchaseStatus.Failure)
            {
                purchases = purchases.Where(t => (t.PurchaseStatus == PurchaseStatus.Success || t.PurchaseStatus == PurchaseStatus.Failure));
            }
        }

        var purchasesEntities = await purchases
            .Skip(request.CurrentPage * request.PageSize)
            .Take(request.PageSize)
            .Include(t => t.Products).ThenInclude(x=>x.Product)
            .Include(t => t.User)
            .Include(t => t.Promocode)
            .ToListAsync(cancellationToken);

        return new PaginationModelDTO<Purchase>
        {
            PageSize = request.PageSize,
            CurrentPage = request.CurrentPage,
            Total = total,
            Products = purchasesEntities
        };
    }
}
