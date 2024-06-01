using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoreanSecrets.BL.Behaviors.Auth.SendPhoneVerificationCode;

public class SendPhoneVerificationCodeCommand : IRequest
{
    public string PhoneNumber { get; set; }

    public Guid UserId { get; set; }
}
