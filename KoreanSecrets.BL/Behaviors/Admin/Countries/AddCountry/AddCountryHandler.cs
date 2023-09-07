using KoreanSecrets.Domain.DbConnection;
using KoreanSecrets.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoreanSecrets.BL.Behaviors.Admin.Countries.AddCountry;

public class AddCountryHandler : IRequestHandler<AddCountryCommand>
{
    private readonly DataContext _context;

    public AddCountryHandler(DataContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(AddCountryCommand request, CancellationToken cancellationToken)
    {
        var country = new Country
        {
            Title = request.Title,
            CategoryId = request.CategoryId
        };

        await _context.Countries.AddAsync(country, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
