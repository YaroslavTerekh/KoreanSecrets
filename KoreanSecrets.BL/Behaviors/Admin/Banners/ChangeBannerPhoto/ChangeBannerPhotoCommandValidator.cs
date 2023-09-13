using FluentValidation;
using KoreanSecrets.Domain.Common.Constants;
using KoreanSecrets.Domain.DbConnection;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoreanSecrets.BL.Behaviors.Admin.Banners.ChangeBannerPhoto;

public class ChangeBannerPhotoCommandValidator : AbstractValidator<ChangeBannerPhotoCommand>
{
    public ChangeBannerPhotoCommandValidator(DataContext context)
    {
        RuleFor(t => t.BannerId)
            .NotEmpty()
            .WithMessage(ValidationMessages.IdRequired)
            .MustAsync(async (id, cancellationToken) =>
            {
                return await context.Banners.AnyAsync(t => t.Id == id, cancellationToken);
            })
            .WithMessage(ErrorMessages.BannerNotFound);
    }
}
