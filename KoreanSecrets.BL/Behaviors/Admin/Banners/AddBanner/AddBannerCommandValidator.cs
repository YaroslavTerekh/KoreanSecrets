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
        RuleFor(t => t.BrandId)
            .NotEmpty()
            .WithMessage(ValidationMessages.IdRequired);

        RuleFor(t => t.Title)
            .NotEmpty()
            .WithMessage(ValidationMessages.TitleRequired);
    }
}
