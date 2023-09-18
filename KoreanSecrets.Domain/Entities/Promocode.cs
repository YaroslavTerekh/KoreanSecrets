using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoreanSecrets.Domain.Entities;

public class Promocode : BaseEntity
{
    public string Code { get; set; }

    public double Discount { get; set; }
}
