using KoreanSecrets.Domain.DbConnection;
using KoreanSecrets.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoreanSecrets.BL.Behaviors.Admin.Products.GetPurchases;

public class GetPurchasesHandler : IRequestHandler<GetPurchasesQuery, List<Purchase>>
{
    private readonly DataContext _context;

    public GetPurchasesHandler(DataContext context)
    {
        _context = context;
    }

    public async Task<List<Purchase>> Handle(GetPurchasesQuery request, CancellationToken cancellationToken)
    {
        var purchases = await _context.Purchases.ToListAsync(cancellationToken);

        return purchases;
    }
}
