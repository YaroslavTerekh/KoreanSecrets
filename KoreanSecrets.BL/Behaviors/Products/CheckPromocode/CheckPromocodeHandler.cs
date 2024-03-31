using KoreanSecrets.Domain.Common.Constants;
using KoreanSecrets.Domain.Common.CustomExceptions;
using KoreanSecrets.Domain.DbConnection;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoreanSecrets.BL.Behaviors.Products.CheckPromocode;

public class CheckPromocodeHandler : IRequestHandler<CheckPromocodeCommand, bool>
{
    private readonly DataContext _context;

    public CheckPromocodeHandler(DataContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(CheckPromocodeCommand request, CancellationToken cancellationToken)
    {
        var promocode = await _context.Promocodes
            .FirstOrDefaultAsync(t => t.Code == request.Promocode && t.IsActive, cancellationToken);

        return promocode is not null;
    }
}
