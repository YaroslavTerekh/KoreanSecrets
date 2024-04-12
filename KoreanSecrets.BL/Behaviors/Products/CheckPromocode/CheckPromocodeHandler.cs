using KoreanSecrets.Domain.Common.Constants;
using KoreanSecrets.Domain.Common.CustomExceptions;
using KoreanSecrets.Domain.DbConnection;
using KoreanSecrets.Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoreanSecrets.BL.Behaviors.Products.CheckPromocode;

public class CheckPromocodeHandler : IRequestHandler<CheckPromocodeCommand, PromocodeUI?>
{
    private readonly DataContext _context;

    public CheckPromocodeHandler(DataContext context)
    {
        _context = context;
    }

    public async Task<PromocodeUI?> Handle(CheckPromocodeCommand request, CancellationToken cancellationToken)
    {
        var promocode = await _context.Promocodes
            .FirstOrDefaultAsync(t => t.Code == request.Promocode && t.IsActive, cancellationToken);

        return promocode is null ? null : new PromocodeUI
        {
            Id = promocode.Id,
            Discount = promocode.Discount,
            Title = promocode.Code
        };
    }
}
