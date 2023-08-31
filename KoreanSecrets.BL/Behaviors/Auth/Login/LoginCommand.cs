using KoreanSecrets.Domain.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoreanSecrets.BL.Behaviors.Auth.Login;

public class LoginCommand : IRequest<AuthToken>
{
    public string PhoneNumber { get; set; }

    public string Password { get; set; }
}
