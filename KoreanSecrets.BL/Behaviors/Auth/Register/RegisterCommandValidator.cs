using FluentValidation;
using KoreanSecrets.Domain.Common.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoreanSecrets.BL.Behaviors.Auth.Register;

public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
{
    public RegisterCommandValidator()
    {
        RuleFor(t => t.FirstName)
            .MinimumLength(2)
            .WithMessage(ValidationMessages.FirstNameTooShort)
            .MaximumLength(20)
            .WithMessage(ValidationMessages.FirstNameTooLong)
            .NotEmpty()
            .WithMessage(ValidationMessages.FirstNameRequired);

        RuleFor(t => t.LastName)
            .MinimumLength(2)
            .WithMessage(ValidationMessages.LastNameTooShort)
            .MaximumLength(20)
            .WithMessage(ValidationMessages.LastNameTooLong)
            .NotEmpty()
            .WithMessage(ValidationMessages.LastNameRequired);

        RuleFor(t => t.Email)
            .EmailAddress()
            .WithMessage(ValidationMessages.WrongEmail)
            .NotEmpty()
            .WithMessage(ValidationMessages.EmailRequired);

        RuleFor(t => t.PhoneNumber)
            .NotEqual(0)
            .WithMessage(ValidationMessages.PhoneNumberRequired)
            .NotEmpty()
            .WithMessage(ValidationMessages.PhoneNumberRequired);
    }
}
