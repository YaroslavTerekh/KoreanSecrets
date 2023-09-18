using FluentValidation;
using KoreanSecrets.Domain.Common.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoreanSecrets.BL.Behaviors.Auth.Login;

public class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(t => t.PhoneNumber)
            .MaximumLength(13)
            .WithMessage(ValidationMessages.PhoneNumberTooLong)
            .MinimumLength(9)
            .WithMessage(ValidationMessages.PhoneNumberTooShort)
            .NotEmpty()
            .WithMessage(ValidationMessages.PhoneNumberRequired);
    }
}
