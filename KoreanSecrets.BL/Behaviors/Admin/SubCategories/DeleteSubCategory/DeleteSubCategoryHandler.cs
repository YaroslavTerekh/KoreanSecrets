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

namespace KoreanSecrets.BL.Behaviors.Admin.SubCategories.DeleteSubCategory;

public class DeleteSubCategoryHandler : IRequestHandler<DeleteSubCategoryCommand>
{
    private readonly DataContext _context;

    public DeleteSubCategoryHandler(DataContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteSubCategoryCommand request, CancellationToken cancellationToken)
    {
        var subCategory = await _context.SubCategories
            .Include(t => t.Products)
            .FirstOrDefaultAsync(t => t.Id == request.SubCategoryId, cancellationToken);

        if (subCategory is null)
            throw new NotFoundException(ErrorMessages.SubCatNotFound);

        if (subCategory.Products.Count > 0)
            throw new DeleteException(ErrorMessages.DeleteProductsFirst(EntityErrorType.SubCategory, subCategory.Title));

        _context.SubCategories.Remove(subCategory);
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
