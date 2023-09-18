using FluentValidation;
using KoreanSecrets.Domain.Common.Constants;
using KoreanSecrets.Domain.DbConnection;
using KoreanSecrets.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoreanSecrets.BL.Behaviors.Auth.Register;

public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
{
    public RegisterCommandValidator(UserManager<User> context)
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
            .MustAsync(async (phoneNumber, cancellationToken) =>
            {
                var exists = await context.Users.AnyAsync(t => t.PhoneNumber == phoneNumber, cancellationToken);
                return !exists;
            })
            .WithMessage(ValidationMessages.UserWithNumberExists)
            .MaximumLength(13)
            .WithMessage(ValidationMessages.PhoneNumberTooLong)
            .MinimumLength(9)
            .WithMessage(ValidationMessages.PhoneNumberTooShort)
            .NotEmpty()
            .WithMessage(ValidationMessages.PhoneNumberRequired);
    }
}
