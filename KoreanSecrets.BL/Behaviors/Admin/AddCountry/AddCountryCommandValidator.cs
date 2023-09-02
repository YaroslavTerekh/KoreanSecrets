using FluentValidation;
using KoreanSecrets.Domain.Common.Constants;
using KoreanSecrets.Domain.DbConnection;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoreanSecrets.BL.Behaviors.Admin.AddCountry;

public class AddCountryCommandValidator : AbstractValidator<AddCountryCommand>
{
    public AddCountryCommandValidator(DataContext context)
    {
        RuleFor(t => t.Title)
            .MinimumLength(4)
            .WithMessage(ValidationMessages.TitleTooShort)
            .MaximumLength(30)
            .WithMessage(ValidationMessages.TitleTooLong)
            .NotEmpty()
            .WithMessage(ValidationMessages.TitleRequired);

        RuleFor(t => t.CategoryId)
            .MustAsync(async (id, cancellationToken) =>
            {
                var exists = await context.Categories.AnyAsync(t => t.Id == id, cancellationToken);
                return exists;
            })
            .WithMessage(ErrorMessages.CategoryNotFound);
    }
}
