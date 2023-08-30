using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace KoreanSecrets.BL.Behaviors.Auth.ChangePassword;

public class ChangePasswordCommand : IRequest
{
    [JsonIgnore]
    public Guid UserId { get; set; }

    public string OldPassword { get; set; }

    public string NewPassword { get; set; }
}
