using KoreanSecrets.Domain.Common.Enums;
using MediatR;

namespace KoreanSecrets.BL.Behaviors.Admin.Users.SendSMS;

public class SendSMSCommand : IRequest
{
    public string PhoneNumber { get; set; }

    public string Text { get; set; }
}
