using KoreanSecrets.Domain.DbConnection;
using KoreanSecrets.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoreanSecrets.BL.Behaviors.Admin.Promocodes.AddPromocode;

public class AddPromocodeHandler : IRequestHandler<AddPromocodeCommand>
{
    private readonly DataContext _context;

    public AddPromocodeHandler(DataContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(AddPromocodeCommand request, CancellationToken cancellationToken)
    {
        var promocode = new Promocode
        {
            Code = request.Title,
            Discount = request.Discount
        };

        await _context.Promocodes.AddAsync(promocode, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
