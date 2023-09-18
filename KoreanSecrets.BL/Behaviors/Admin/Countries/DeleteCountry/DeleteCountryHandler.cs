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

namespace KoreanSecrets.BL.Behaviors.Admin.Countries.DeleteCountry;

public class DeleteCountryHandler : IRequestHandler<DeleteCountryCommand>
{
    private readonly DataContext _context;

    public DeleteCountryHandler(DataContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteCountryCommand request, CancellationToken cancellationToken)
    {
        var country = await _context.Countries
            .Include(t => t.Products)
            .FirstOrDefaultAsync(t => t.Id == request.CountryId, cancellationToken);

        if (country is null)
            throw new NotFoundException(ErrorMessages.CountryNotFound);

        if (country.Products.Count > 0)
            throw new DeleteException(ErrorMessages.DeleteProductsFirst(EntityErrorType.Country, country.Title));

        _context.Countries.Remove(country);
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
