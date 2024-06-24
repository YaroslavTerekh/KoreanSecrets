using FluentValidation;
using KoreanSecrets.Domain.Common.Constants;
using KoreanSecrets.Domain.DbConnection;

namespace KoreanSecrets.BL.Behaviors.Admin.Products.MainData.AddProduct;

public class AddProductCommandValidator : AbstractValidator<AddProductCommand>
{
    public AddProductCommandValidator(DataContext context)
    {
        RuleFor(t => t.Title)
            .NotEmpty()
            .WithMessage(ValidationMessages.TitleRequired);
    }
}
