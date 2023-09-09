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

namespace KoreanSecrets.BL.Behaviors.Admin.SubCategories.ModifySubCategory;

public class ModifySubCategoryHandler : IRequestHandler<ModifySubCategoryCommand>
{
    private readonly DataContext _context;

    public ModifySubCategoryHandler(DataContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(ModifySubCategoryCommand request, CancellationToken cancellationToken)
    {
        var subCategory = await _context.SubCategories.FirstOrDefaultAsync(t => t.Id == request.Id, cancellationToken);

        if (subCategory is null)
            throw new NotFoundException(ErrorMessages.SubCatNotFound);

        subCategory.Title = request.Title;
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}

