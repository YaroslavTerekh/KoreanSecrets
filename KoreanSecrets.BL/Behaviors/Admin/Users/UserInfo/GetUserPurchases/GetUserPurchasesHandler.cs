using KoreanSecrets.Domain.Common.Enums;
using KoreanSecrets.Domain.DataTransferObjects;
using KoreanSecrets.Domain.DbConnection;
using KoreanSecrets.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KoreanSecrets.BL.Behaviors.Admin.Users.UserInfo.GetUserPurchases;

public class GetUserPurchasesHandler : IRequestHandler<GetUserPurchasesQuery, PaginationModelDTO<Purchase>>
{
    private readonly DataContext _context;

    public GetUserPurchasesHandler(DataContext context)
    {
        _context = context;
    }

    public async Task<PaginationModelDTO<Purchase>> Handle(GetUserPurchasesQuery request, CancellationToken cancellationToken)
    {
        var purchases = _context.Purchases
            .Where(x=>x.UserId == request.UserId)
            .Include(x=>x.Products)
            .OrderBy(t => t.CreatedDate)
            .AsQueryable();

        // if(request.Status is not null)
        // {
        //     if (request.Status == PurchaseStatus.New)
        //     {
        //         purchases = purchases.Where(t => (t.PurchaseStatus == PurchaseStatus.New || t.PurchaseStatus == PurchaseStatus.Waiting));
        //     }
        //     
        //     if (request.Status == PurchaseStatus.Success)
        //     {
        //         purchases = purchases.Where(t => (t.PurchaseStatus == PurchaseStatus.Success || t.PurchaseStatus == PurchaseStatus.Failure));
        //     }
        // }

        if (!string.IsNullOrEmpty(request.ColumnToSort))
        {
            purchases = request.ColumnToSort switch
            {
                "name" => request?.WayToSort == "asc"
                    ? purchases.OrderBy(t => t.Products.OrderBy(p => p.ProductTitle))
                    : purchases.OrderByDescending(t => t.Products.OrderByDescending(p => p.ProductTitle)),
                "price" => request?.WayToSort == "asc"
                    ? purchases.OrderBy(t => t.TotalPrice)
                    : purchases.OrderByDescending(t => t.TotalPrice),
                "contacts" => request?.WayToSort == "asc"
                    ? purchases.OrderBy(t => t.User.PhoneNumber)
                    : purchases.OrderByDescending(t => t.User.PhoneNumber),
                "address" => request?.WayToSort == "asc"
                    ? purchases.OrderBy(t => t.City).ThenBy(x=>x.Warehouse)
                    : purchases.OrderByDescending(t => t.City).OrderByDescending(x=>x.Warehouse),
                "comment" => request?.WayToSort == "asc"
                    ? purchases.OrderBy(t => t.Comment)
                    : purchases.OrderByDescending(t => t.Comment),
                "payType" => request?.WayToSort == "asc"
                    ? purchases.OrderBy(t => t.PayType)
                    : purchases.OrderByDescending(t => t.PayType),
                "status" => request?.WayToSort == "asc"
                    ? purchases.OrderBy(t => t.PurchaseStatus)
                    : purchases.OrderByDescending(t => t.PurchaseStatus),
                _ => purchases
            };
        }
        else
        {
            purchases = purchases.OrderByDescending(x => x.CreatedDate);
        }
        
        var total = await purchases.CountAsync(cancellationToken);

        var purchasesEntities = await purchases
            .Skip(request.CurrentPage * request.PageSize)
            .Take(request.PageSize)
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
