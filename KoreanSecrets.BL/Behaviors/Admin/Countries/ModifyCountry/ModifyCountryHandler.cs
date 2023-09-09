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

namespace KoreanSecrets.BL.Behaviors.Admin.Countries.ModifyCountry;

public class ModifyCountryHandler : IRequestHandler<ModifyCountryCommand>
{
    private readonly DataContext _context;

    public ModifyCountryHandler(DataContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(ModifyCountryCommand request, CancellationToken cancellationToken)
    {
        var country = await _context.Countries.FirstOrDefaultAsync(t => t.Id == request.Id, cancellationToken);

        if (country is null)
            throw new NotFoundException(ErrorMessages.CountryNotFound);

        country.Title = request.Title;
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}

