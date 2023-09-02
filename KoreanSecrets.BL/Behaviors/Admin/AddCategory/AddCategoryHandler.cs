using KoreanSecrets.Domain.DbConnection;
using KoreanSecrets.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoreanSecrets.BL.Behaviors.Admin.AddCategory;

public class AddCategoryHandler : IRequestHandler<AddCategoryCommand>
{
    private readonly DataContext _context;

    public AddCategoryHandler(DataContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(AddCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = new Category
        {
            Title = request.Title
        };

        await _context.Categories.AddAsync(category, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
