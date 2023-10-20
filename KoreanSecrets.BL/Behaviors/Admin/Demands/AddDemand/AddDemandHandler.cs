using KoreanSecrets.Domain.DbConnection;
using KoreanSecrets.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoreanSecrets.BL.Behaviors.Admin.Demands.AddDemand;

public class AddDemandHandler : IRequestHandler<AddDemandCommand>
{
    private readonly DataContext _context;

    public AddDemandHandler(DataContext context)
    {
        _context = context;
    }

    //TODO: check-fix
    public async Task<Unit> Handle(AddDemandCommand request, CancellationToken cancellationToken)
    {
        var demand = new Demand
        {
            Title = request.Title
        };

        var categoryDemand = new CategoryDemand
        {
            DemandId = demand.Id,
            CategoryId = request.CategoryId
        };

        await _context.Demands.AddAsync(demand, cancellationToken);
        await _context.CategoryDemands.AddAsync(categoryDemand, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
