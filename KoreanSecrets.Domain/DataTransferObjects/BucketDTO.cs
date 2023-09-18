using KoreanSecrets.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoreanSecrets.Domain.DataTransferObjects;

public class BucketDTO : BaseEntity
{
    public List<PurchaseProductDTO> PurchaseProducts { get; set; }
}

public class PurchaseProductDTO : BaseEntity
{
    public ListProductDTO Product { get; set; }

    public int Amount { get; set; }
}
