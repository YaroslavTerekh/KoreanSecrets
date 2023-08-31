using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoreanSecrets.Domain.Entities;

public class AddressInfo : BaseEntity
{
    public Guid UserId { get; set; }

    public User User { get; set; }

    public string City { get; set; }

    public string Street { get; set; }

    public string HouseNumber { get; set; }

    public long? FlatNumber { get; set; }

    public string ZipCode { get; set; }
}
