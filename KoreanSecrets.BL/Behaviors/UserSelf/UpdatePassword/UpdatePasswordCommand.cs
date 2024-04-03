using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoreanSecrets.BL.Behaviors.UserSelf.UpdatePassword;

public class UpdatePasswordCommand : IAuthorizedRequest
{
    public string OldPassword { get; set; }

    public string Password { get; set; }

    public string ConfirmPassword { get; set; }
}
