using KoreanSecrets.Domain.Common.Constants;
using KoreanSecrets.Domain.Common.CustomExceptions;
using KoreanSecrets.Domain.DbConnection;
using KoreanSecrets.Domain.Entities;
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

        product.BrandId = request.BrandId == Guid.Empty ? null : request.BrandId;
        product.CategoryId = request.CategoryId == Guid.Empty ? null : request.CategoryId;
        product.CountryId = request.CountryId == Guid.Empty ? null : request.CountryId;
        product.SubCategoryId = request.SubCategoryId == Guid.Empty ? null : request.SubCategoryId;
        product.Title = request.Title;
        product.Characteristics = request.Characteristics;
        product.Syllabes = request.Syllabes;
        product.Usage = request.Usage;
        product.AdditionalIcon = request.Icon;
        product.Quantity = request.Quantity;

        var newProductDemands = new List<ProductDemand>();

        foreach(var id in request.DemandId)
        {
            var productDemand = new ProductDemand
            {
                DemandId = id,
                ProductId = product.Id
            };

            if (product?.ProductDemands?.FirstOrDefault(x => x.DemandId == id) == null)
            {
                newProductDemands.Add(productDemand);
            }
        }

        await _context.ProductDemand.AddRangeAsync(newProductDemands, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
