using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoreanSecrets.Domain.Entities;

public class PurchasedProduct : BaseEntity
{
    public string ProductIdentify { get; set; }

    public string Product { get; set; }

    public string ProductTitle { get; set; }

    public Guid PurchaseId { get; set; }

    public Purchase Purchase { get; set; }

    public string VolumeIdentify { get; set; }

    public string Volume { get; set; }

    public int Amount { get; set; }
}
