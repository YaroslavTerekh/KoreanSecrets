using KoreanSecrets.Domain.DbConnection;
using KoreanSecrets.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoreanSecrets.BL.Behaviors.Admin.Promotions.GetAllPromotions;

public class GetAllPromotionsHandler : IRequestHandler<GetAllPromotionsQuery, List<Promotion>>
{
    private readonly DataContext _context;

    public GetAllPromotionsHandler(DataContext context)
    {
        _context = context;
    }

    public async Task<List<Promotion>> Handle(GetAllPromotionsQuery request, CancellationToken cancellationToken)
    {
        return await _context.Promotions
            .Include(t => t.Brand)
            .ToListAsync();
    }
}
