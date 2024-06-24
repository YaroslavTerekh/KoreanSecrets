using FluentValidation;
using KoreanSecrets.Domain.Common.Constants;
using KoreanSecrets.Domain.DbConnection;
using Microsoft.EntityFrameworkCore;

namespace KoreanSecrets.BL.Behaviors.Admin.Products.Discounts.AddDiscount;

public class AddDiscountCommandValidator : AbstractValidator<AddDiscountCommand>
{
    public AddDiscountCommandValidator(DataContext context)
    {
        RuleFor(t => t.ProductId)
            .MustAsync(async (id, cancellationToken) =>
            {
                return await context.Products.AnyAsync(t => t.Id == id, cancellationToken);
            })
            .WithMessage(ErrorMessages.SomeProductNotFound);

        RuleFor(t => t.NewPrice)
            .NotEmpty()
            .WithMessage(ErrorMessages.WrongNewPrice);
    }
}
