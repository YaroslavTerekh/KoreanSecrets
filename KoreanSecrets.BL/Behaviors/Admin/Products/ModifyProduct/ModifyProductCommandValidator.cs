using FluentValidation;
using KoreanSecrets.Domain.Common.Constants;
using KoreanSecrets.Domain.DbConnection;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoreanSecrets.BL.Behaviors.Admin.Products.ModifyProduct;

public class ModifyProductCommandValidator : AbstractValidator<ModifyProductCommand>
{
    public ModifyProductCommandValidator(DataContext context)
    {
        RuleFor(t => t.Title)
            .MinimumLength(5)
            .WithMessage(ValidationMessages.TitleTooShort)
            .MaximumLength(200)
            .WithMessage(ValidationMessages.TitleTooLong)
            .NotEmpty()
            .WithMessage(ValidationMessages.TitleRequired);
    }
}
