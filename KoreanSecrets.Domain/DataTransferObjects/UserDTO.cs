using KoreanSecrets.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoreanSecrets.Domain.DataTransferObjects;

public class UserDTO : BaseEntity
{
    public string FirstName { get; set; }

    public string LastName { get; set; }
}
