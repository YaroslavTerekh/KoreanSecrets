using KoreanSecrets.Domain.Common.Constants;
using KoreanSecrets.Domain.Common.CustomExceptions;
using KoreanSecrets.Domain.DbConnection;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KoreanSecrets.BL.Behaviors.Admin.Products.ModifyProduct;

public class ModifyProductHandler : IRequestHandler<ModifyProductCommand>
{
    private readonly DataContext _context;

    public ModifyProductHandler(DataContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(ModifyProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _context.Products
            .Where(t => t.Id == request.ProductId)
            .FirstOrDefaultAsync(cancellationToken);

        if (product is null)
            throw new NotFoundException(ErrorMessages.SomeProductNotFound);

        product.BrandId = request.BrandId;
        product.CategoryId = request.CategoryId;
        product.CountryId = request.CountryId;
        product.DemandId = request.DemandId;
        product.SubCategoryId = request.SubCategoryId;
        product.Title = request.Title;
        product.Characteristics = request.Characteristics;
        product.Syllabes = request.Syllabes;
        product.Usage = request.Usage;
        product.Price = request.Price;
        product.AdditionalIcon = request.Icon;

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
