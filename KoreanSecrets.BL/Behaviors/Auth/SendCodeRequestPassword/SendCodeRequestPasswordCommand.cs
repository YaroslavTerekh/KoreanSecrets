using MediatR;

namespace KoreanSecrets.BL.Behaviors.Auth.SendCodeRequestPassword;

public class SendCodeRequestPasswordCommand : IRequest
{
    public string PhoneNumber { get; set; }
}
