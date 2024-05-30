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
}
