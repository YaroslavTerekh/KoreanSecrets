using KoreanSecrets.Domain.DbConnection;
using KoreanSecrets.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoreanSecrets.BL.Behaviors.Admin.AddSubCategory;

public class AddSubCategoryHandler : IRequestHandler<AddSubCategoryCommand>
{
    private readonly DataContext _context;

    public AddSubCategoryHandler(DataContext context)
    { 
        _context = context;
    }

    public async Task<Unit> Handle(AddSubCategoryCommand request, CancellationToken cancellationToken)
    {
        var subCategory = new SubCategory
        {
            Title = request.Title,
            CategoryId = request.CategoryId
        };

        await _context.SubCategories.AddAsync(subCategory, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
