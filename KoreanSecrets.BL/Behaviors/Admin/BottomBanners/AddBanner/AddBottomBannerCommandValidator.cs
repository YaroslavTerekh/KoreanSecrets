using FluentValidation;
using KoreanSecrets.BL.Behaviors.Admin.Banners.AddBanner;
using KoreanSecrets.Domain.Common.Constants;
using KoreanSecrets.Domain.DbConnection;

namespace KoreanSecrets.BL.Behaviors.Admin.BottomBanners.AddBanner;

public class AddBottomBannerCommandValidator : AbstractValidator<AddBottomBannerCommand>
{
    public AddBottomBannerCommandValidator(DataContext context)
    {
        RuleFor(t => t.FirstPhoto)
            .NotEmpty()
            .WithMessage(ValidationMessages.PhotoRequired);

        RuleFor(t => t.SecondPhoto)
            .NotEmpty()
            .WithMessage(ValidationMessages.PhotoRequired);
    }
}
