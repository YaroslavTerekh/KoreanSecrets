using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace KoreanSecrets.Domain.Common.CustomExceptions;

public class AuthException : Exception
{
    public HttpStatusCode Code { get; set; } = HttpStatusCode.BadRequest;

    public IEnumerable<IdentityError> Errors { get; set; }

    public string Description { get; set; }


    public AuthException(HttpStatusCode code, IEnumerable<IdentityError> errors) : base()
    {
        Code = code;
        Errors = errors;
    }

    public AuthException(HttpStatusCode code, string error) : base()
    {
        Code = code;
        Description = error;
    }
}
