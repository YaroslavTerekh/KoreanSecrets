using KoreanSecrets.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoreanSecrets.Domain.DataTransferObjects;

public class AddressInfoDTO : BaseEntity
{
    public string City { get; set; }

    public string Warehouse { get; set; }
}
