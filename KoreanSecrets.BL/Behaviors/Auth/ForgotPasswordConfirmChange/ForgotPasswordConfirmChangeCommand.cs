using System.Text.Json.Serialization;
using MediatR;

namespace KoreanSecrets.BL.Behaviors.Auth.ForgotPasswordConfirmChange;

public class ForgotPasswordConfirmChangeCommand : IRequest
{
    public string PhoneNumber { get; set; }
    public string NewPassword { get; set; }
    
    public long ConfirmationCode { get; set; }
}
