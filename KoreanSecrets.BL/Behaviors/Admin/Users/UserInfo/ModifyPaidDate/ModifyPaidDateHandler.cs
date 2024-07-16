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

namespace KoreanSecrets.BL.Behaviors.Admin.Users.UserInfo.ModifyPaidDate;

public class ModifyPaidDateHandler : IRequestHandler<ModifyPaidDateCommand>
{
    private readonly DataContext _context;

    public ModifyPaidDateHandler(DataContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(ModifyPaidDateCommand request, CancellationToken cancellationToken)
    {
        var purchase = await _context.Purchases.FirstOrDefaultAsync(t => t.PurchaseIdentifier == request.PurchaseId, cancellationToken);

        if(purchase is null)
            throw new NotFoundException(ErrorMessages.PurchaseNotFound);

        if (request.PaidDate.HasValue)
        {
            request.PaidDate = request.PaidDate.Value.AddHours(10);
        }
        
        purchase.PaidDate = request.PaidDate;
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
