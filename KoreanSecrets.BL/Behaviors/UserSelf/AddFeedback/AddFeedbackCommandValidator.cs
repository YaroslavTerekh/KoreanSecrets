using FluentValidation;
using KoreanSecrets.Domain.Common.Constants;
using KoreanSecrets.Domain.DbConnection;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoreanSecrets.BL.Behaviors.UserSelf.AddFeedback;

public class AddFeedbackCommandValidator : AbstractValidator<AddFeedbackCommand>
{
    public AddFeedbackCommandValidator(DataContext context)
    {
        RuleFor(t => t.FeedbackText)
            .MinimumLength(20)
            .WithMessage(ValidationMessages.FeedbackTooShort)
            .MaximumLength(200)
            .WithMessage(ValidationMessages.FeedbackTooLong)
            .NotEmpty()
            .WithMessage(ValidationMessages.FeedbackRequired);

        RuleFor(t => t.ProductId)
            .MustAsync(async (id, cancellationToken) =>
            {
                return await context.Products.AnyAsync(t => t.Id == id, cancellationToken);
            })
            .WithMessage(ErrorMessages.SomeProductNotFound);
    }
}
