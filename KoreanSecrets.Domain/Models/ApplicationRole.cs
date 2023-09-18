using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoreanSecrets.Domain.Models;

public class ApplicationRole : IdentityRole<Guid>
{
    public ApplicationRole(string name) : base(name) { }
}
