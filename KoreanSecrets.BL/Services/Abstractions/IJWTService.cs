using KoreanSecrets.Domain.Entities;
using KoreanSecrets.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoreanSecrets.BL.Services.Abstractions;

public interface IJWTService
{
    public AuthToken GenerateJWT(User user, string[] roles);
}
