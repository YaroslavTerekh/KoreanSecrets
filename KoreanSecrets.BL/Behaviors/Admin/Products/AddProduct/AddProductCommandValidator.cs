using FluentValidation;
using KoreanSecrets.Domain.Common.Constants;
using KoreanSecrets.Domain.DbConnection;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoreanSecrets.BL.Behaviors.Admin.Products.AddProduct;

public class AddProductCommandValidator : AbstractValidator<AddProductCommand>
{
    public AddProductCommandValidator(DataContext context)
    {
        RuleFor(t => t.Title)
            .MinimumLength(5)
            .WithMessage(ValidationMessages.TitleTooShort)
            .MaximumLength(200)
            .WithMessage(ValidationMessages.TitleTooLong)
            .NotEmpty()
            .WithMessage(ValidationMessages.TitleRequired);

        RuleFor(t => t.Characteristics)
            .MinimumLength(100)
            .WithMessage(ValidationMessages.DescriptionTooShort)
            .MaximumLength(3000)
            .WithMessage(ValidationMessages.DescriptionTooLong)
            .NotEmpty()
            .WithMessage(ValidationMessages.DescriptionRequired);

        RuleFor(t => t.Syllabes)
            .MinimumLength(100)
            .WithMessage(ValidationMessages.DescriptionTooShort)
            .MaximumLength(3000)
            .WithMessage(ValidationMessages.DescriptionTooLong)
            .NotEmpty()
            .WithMessage(ValidationMessages.DescriptionRequired);

        RuleFor(t => t.Usage)
            .MinimumLength(100)
            .WithMessage(ValidationMessages.DescriptionTooShort)
            .MaximumLength(3000)
            .WithMessage(ValidationMessages.DescriptionTooLong)
            .NotEmpty()
            .WithMessage(ValidationMessages.DescriptionRequired);

        RuleFor(t => t.BrandId)
            .MustAsync(async (id, cancellationToken) =>
            {
                var exists = await context.Brands.AnyAsync(t => t.Id == id, cancellationToken);
                return exists;
            })
            .WithMessage(ErrorMessages.BrandNotFound);

        RuleFor(t => t.CategoryId)
            .MustAsync(async (id, cancellationToken) =>
            {
                var exists = await context.Categories.AnyAsync(t => t.Id == id, cancellationToken);
                return exists;
            })
            .WithMessage(ErrorMessages.CategoryNotFound);

        RuleFor(t => t.SubCategoryId)
            .MustAsync(async (id, cancellationToken) =>
            {
                var exists = await context.SubCategories.AnyAsync(t => t.Id == id, cancellationToken);
                return exists;
            })
            .WithMessage(ErrorMessages.SubCatNotFound);

        RuleFor(t => t.DemandId)
            .MustAsync(async (id, cancellationToken) =>
            {
                var exists = await context.Demands.AnyAsync(t => t.Id == id, cancellationToken);
                return exists;
            })
            .WithMessage(ErrorMessages.DemandNotFound);

        RuleFor(t => t.CountryId)
            .MustAsync(async (id, cancellationToken) =>
            {
                var exists = await context.Countries.AnyAsync(t => t.Id == id, cancellationToken);
                return exists;
            })
            .WithMessage(ErrorMessages.CountryNotFound);
    }
}
