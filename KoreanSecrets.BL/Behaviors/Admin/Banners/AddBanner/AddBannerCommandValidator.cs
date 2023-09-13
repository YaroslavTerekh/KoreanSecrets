using FluentValidation;
using KoreanSecrets.Domain.Common.Constants;
using KoreanSecrets.Domain.DbConnection;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoreanSecrets.BL.Behaviors.Admin.Banners.AddBanner;

public class AddBannerCommandValidator : AbstractValidator<AddBannerCommand>
{
    public AddBannerCommandValidator(DataContext context)
    {
        RuleFor(t => t.ProductId)
            .NotEmpty()
            .WithMessage(ValidationMessages.IdRequired)
            .MustAsync(async (id, cancellationToken) =>
            {
                return await context.Products.AnyAsync(t => t.Id == id, cancellationToken);
            })
            .WithMessage(ErrorMessages.SomeProductNotFound);

        RuleFor(t => t.Title)
            .NotEmpty()
            .WithMessage(ValidationMessages.TitleRequired)
            .MinimumLength(5)
            .WithMessage(ValidationMessages.TitleTooShort)
            .MaximumLength(100)
            .WithMessage(ValidationMessages.TitleTooLong);
    }
}
