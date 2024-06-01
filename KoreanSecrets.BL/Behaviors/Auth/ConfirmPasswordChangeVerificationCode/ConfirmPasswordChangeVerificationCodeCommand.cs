using KoreanSecrets.Domain.Models;
using MediatR;

namespace KoreanSecrets.BL.Behaviors.Auth.ConfirmPasswordChangeVerificationCode;

public class ConfirmPasswordChangeVerificationCodeCommand : IRequest<AuthToken>
{
    public string PhoneNumber { get; set; }

    public long ConfirmationCode { get; set; }
}
