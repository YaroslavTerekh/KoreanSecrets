using KoreanSecrets.Domain.DbConnection;
using KoreanSecrets.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoreanSecrets.BL.Behaviors.Admin.Brands.AddBrand;

public class AddBrandHandler : IRequestHandler<AddBrandCommand>
{
    private readonly DataContext _context;

    public AddBrandHandler(DataContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(AddBrandCommand request, CancellationToken cancellationToken)
    {
        var brand = new Brand
        {
            Title = request.Title,
            CategoryId = request.CategoryId
        };

        await _context.Brands.AddAsync(brand, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
