using FluentValidation;
using KoreanSecrets.Domain.Common.Constants;
using KoreanSecrets.Domain.DbConnection;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoreanSecrets.BL.Behaviors.Admin.SubCategories.AddSubCategory;

public class AddSubCategoryCommandValidator : AbstractValidator<AddSubCategoryCommand>
{
    public AddSubCategoryCommandValidator(DataContext context)
    {
        RuleFor(t => t.Title)
            .MinimumLength(4)
            .WithMessage(ValidationMessages.TitleTooShort)
            .MaximumLength(30)
            .WithMessage(ValidationMessages.TitleTooLong)
            .NotEmpty()
            .WithMessage(ValidationMessages.TitleRequired);
    }
}
