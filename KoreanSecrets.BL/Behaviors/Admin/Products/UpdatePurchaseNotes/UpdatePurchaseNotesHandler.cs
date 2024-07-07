using KoreanSecrets.Domain.Common.Constants;
using KoreanSecrets.Domain.Common.CustomExceptions;
using KoreanSecrets.Domain.Common.Enums;
using KoreanSecrets.Domain.DataTransferObjects;
using KoreanSecrets.Domain.DbConnection;
using KoreanSecrets.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KoreanSecrets.BL.Behaviors.Admin.Products.UpdatePurchaseNotes;

public class UpdatePurchaseNotesHandler : IRequestHandler<UpdatePurchaseNotesQuery>
{
    private readonly DataContext _context;

    public UpdatePurchaseNotesHandler(DataContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdatePurchaseNotesQuery request, CancellationToken cancellationToken)
    {
        var purchase = await _context.Purchases
            .FirstOrDefaultAsync(x => x.PurchaseIdentifier == request.Id, cancellationToken);

        if(purchase is null)
        {
            throw new NotFoundException(ErrorMessages.PurchaseNotFound);
        }

        purchase.AdminNotes = request.Notes;

        _context.Purchases.Update(purchase);
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
