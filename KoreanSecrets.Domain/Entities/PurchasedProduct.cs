using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoreanSecrets.Domain.Entities;

public class PurchasedProduct : BaseEntity
{
    public Guid ProductId { get; set; }

    public Product Product { get; set; }

    public Guid BucketId { get; set; }

    public Bucket Bucket { get; set; }

    public int Amount { get; set; }
}
