using FluentValidation;
using KoreanSecrets.Domain.Common.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoreanSecrets.BL.Behaviors.UserSelf.ModifyAddressInfo;

public class ModifyAddressInfoCommandValidator : AbstractValidator<ModifyAddressInfoCommand>
{
    public ModifyAddressInfoCommandValidator()
    {
        RuleFor(t => t.City)
            .MinimumLength(1)
            .WithMessage(ValidationMessages.CityToShort);

        RuleFor(t => t.Warehouse)
            .MinimumLength(1)
            .WithMessage(ValidationMessages.AddressToShort);
    }
}
