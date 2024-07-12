using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoreanSecrets.Domain.DataTransferObjects;

public class VolumeDTO
{
    public Guid Id { get; set; }
    public string Value { get; set; }

    public string Unit { get; set; }

    public decimal Price { get; set; }
    public decimal? PriceWithDiscount { get; set; }
    public int Quantity { get; set; }
    public List<AppFileDTO> Photos { get; set; }
    public List<VolumeUserDTO> UsersWaitingForStock { get; set; }
}

public class VolumeUserDTO
{
    public Guid VolumeId { get; set; }

    public VolumeDTO Volume { get; set; }

    public Guid UserId { get; set; }

    public UserDTO User { get; set; }
}

