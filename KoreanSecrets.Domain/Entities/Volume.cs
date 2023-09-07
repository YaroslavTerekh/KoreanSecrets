using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoreanSecrets.Domain.Entities;

public class Volume : BaseEntity
{
    public long Value { get; set; }

    public string Unit { get; set; }

    public Guid ProductId { get; set; }

    public Product Product { get; set; }
}
