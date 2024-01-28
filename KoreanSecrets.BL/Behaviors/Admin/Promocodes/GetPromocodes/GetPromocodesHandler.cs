using KoreanSecrets.Domain.DbConnection;
using KoreanSecrets.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoreanSecrets.BL.Behaviors.Admin.Promocodes.GetPromocodes;

public class GetPromocodesHandler : IRequestHandler<GetPromocodesQuery, List<Promocode>>
{
    private readonly DataContext _context;

    public GetPromocodesHandler(DataContext context)
    {
        _context = context;
    }

    public async Task<List<Promocode>> Handle(GetPromocodesQuery request, CancellationToken cancellationToken)
    {
        return await _context.Promocodes.ToListAsync(cancellationToken);
    }
}
