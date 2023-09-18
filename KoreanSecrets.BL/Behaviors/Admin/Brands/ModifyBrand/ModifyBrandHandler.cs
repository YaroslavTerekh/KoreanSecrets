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

namespace KoreanSecrets.BL.Behaviors.Admin.Brands.ModifyBrand;

public class ModifyBrandHandler : IRequestHandler<ModifyBrandCommand>
{
    private readonly DataContext _context;

    public ModifyBrandHandler(DataContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(ModifyBrandCommand request, CancellationToken cancellationToken)
    {
        var brand = await _context.Brands.FirstOrDefaultAsync(t => t.Id == request.Id, cancellationToken);

        if (brand is null)
            throw new NotFoundException(ErrorMessages.BrandNotFound);

        brand.Title = request.Title;
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
