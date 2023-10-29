using KoreanSecrets.Domain.Common.Constants;
using KoreanSecrets.Domain.Common.CustomExceptions;
using KoreanSecrets.Domain.Common.Enums;
using KoreanSecrets.Domain.DbConnection;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoreanSecrets.BL.Behaviors.Admin.Demands.DeleteDemand;

public class DeleteDemandHandler : IRequestHandler<DeleteDemandCommand>
{
    private readonly DataContext _context;

    public DeleteDemandHandler(DataContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteDemandCommand request, CancellationToken cancellationToken)
    {
        var demand = await _context.Demands
            .Include(t => t.Products)
            .FirstOrDefaultAsync(t => t.Id == request.DemandId, cancellationToken);

        if (demand is null)
            throw new NotFoundException(ErrorMessages.DemandNotFound);

        demand.Products.ForEach(t => t.DemandId = Guid.Empty);
        _context.Demands.Remove(demand);
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
