using FluentValidation;
using KoreanSecrets.Domain.Common.Constants;
using KoreanSecrets.Domain.DbConnection;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoreanSecrets.BL.Behaviors.Admin.Products.AddDiscount;

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
            .WithMessage(ErrorMessages.WrongNewPrice)
            .MustAsync(async (command, price, cancellationToken) =>
            {
                var product = await context.Products.FirstOrDefaultAsync(t => t.Id == command.ProductId, cancellationToken);

                if (product is null)
                    throw new ValidationException(ErrorMessages.SomeProductNotFound);

                return price < product.Price;
            })
            .WithMessage(ErrorMessages.WrongNewPrice);
    }
}
