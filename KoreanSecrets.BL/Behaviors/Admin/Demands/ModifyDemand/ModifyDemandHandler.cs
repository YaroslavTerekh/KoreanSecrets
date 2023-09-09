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

namespace KoreanSecrets.BL.Behaviors.Admin.Demands.ModifyDemand;

public class ModifyDemandHandler : IRequestHandler<ModifyDemandCommand>
{
    private readonly DataContext _context;

    public ModifyDemandHandler(DataContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(ModifyDemandCommand request, CancellationToken cancellationToken)
    {
        var demand = await _context.Demands.FirstOrDefaultAsync(t => t.Id == request.Id, cancellationToken);

        if (demand is null)
            throw new NotFoundException(ErrorMessages.DemandNotFound);

        demand.Title = request.Title;
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}

