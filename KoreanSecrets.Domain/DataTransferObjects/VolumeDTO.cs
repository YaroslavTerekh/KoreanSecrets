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

    public long Price { get; set; }
    public int Quantity { get; set; }
    public List<AppFileDTO> Photos { get; set; }
}
