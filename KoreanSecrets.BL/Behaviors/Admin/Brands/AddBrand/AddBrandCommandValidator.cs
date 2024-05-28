using FluentValidation;
using KoreanSecrets.Domain.Common.Constants;
using KoreanSecrets.Domain.DbConnection;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoreanSecrets.BL.Behaviors.Admin.Brands.AddBrand;

public class AddBrandCommandValidator : AbstractValidator<AddBrandCommand>
{
    public AddBrandCommandValidator(DataContext context)
    {
        RuleFor(t => t.Title)
            .NotEmpty()
            .WithMessage(ValidationMessages.TitleRequired);
    }
}
