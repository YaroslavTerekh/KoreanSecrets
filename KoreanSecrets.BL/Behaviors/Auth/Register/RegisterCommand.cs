using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoreanSecrets.BL.Behaviors.Auth.Register;

public class RegisterCommand : IRequest<Guid>
{
    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string Password { get; set; }
}
