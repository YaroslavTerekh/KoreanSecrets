using FluentValidation;
using KoreanSecrets.Domain.Common.Constants;
using KoreanSecrets.Domain.DbConnection;

namespace KoreanSecrets.BL.Behaviors.Admin.Products.MainData.ModifyProduct;

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
