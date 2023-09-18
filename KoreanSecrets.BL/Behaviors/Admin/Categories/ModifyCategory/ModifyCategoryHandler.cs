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

namespace KoreanSecrets.BL.Behaviors.Admin.Categories.ModifyCategory;

public class ModifyCategoryHandler : IRequestHandler<ModifyCategoryCommand>
{
    private readonly DataContext _context;

    public ModifyCategoryHandler(DataContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(ModifyCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await _context.Categories.FirstOrDefaultAsync(t => t.Id == request.Id, cancellationToken);

        if (category is null)
            throw new NotFoundException(ErrorMessages.CategoryNotFound);

        category.Title = request.Title;
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}

