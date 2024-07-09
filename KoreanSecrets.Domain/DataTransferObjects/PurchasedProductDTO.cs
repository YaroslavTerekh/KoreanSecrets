using KoreanSecrets.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoreanSecrets.Domain.DataTransferObjects;

public class PurchasedProductDTO : BaseEntity
{
    public string ProductIdentify { get; set; }

    public string Product { get; set; }

    public string ProductTitle { get; set; }

    public string VolumeIdentify { get; set; }

    public string Volume { get; set; }

    public int Amount { get; set; }
}
