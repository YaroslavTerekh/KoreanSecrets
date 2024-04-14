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

namespace KoreanSecrets.BL.Behaviors.UserSelf.UpdateOrderStatus;

public class UpdateOrderStatusHandler : IRequestHandler<UpdateOrderStatusCommand>
{
    private readonly DataContext _context;

    public UpdateOrderStatusHandler(DataContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdateOrderStatusCommand request, CancellationToken cancellationToken)
    {
        var order = await _context.Purchases.FirstOrDefaultAsync(t => t.Id == request.Id, cancellationToken);

        if (order is null) throw new NotFoundException(ErrorMessages.PurchaseNotFound);

        if (order.UserId != request.CurrentUserId) throw new Exception(ErrorMessages.PurchaseNotRelatedToUser);

        order.PurchaseStatus = request.Status;
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
