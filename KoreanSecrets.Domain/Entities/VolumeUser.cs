using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoreanSecrets.Domain.Entities;

public class VolumeUser
{
    public Guid VolumeId { get; set; }

    public Volume Volume { get; set; }

    public Guid UserId { get; set; }

    public User User { get; set; }
}
