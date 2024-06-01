using KoreanSecrets.Domain.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoreanSecrets.BL.Behaviors.Auth.ConfirmPhoneVerificationCode;

public class ConfirmPhoneVerificationCodeCommand : IRequest<AuthToken>
{
    public string PhoneNumber { get; set; }

    public long ComfirmationCode { get; set; }

    public Guid UserId { get; set; }
}
