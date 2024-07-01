using KoreanSecrets.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoreanSecrets.Domain.DataTransferObjects;

public class PurchaseProductDTO : BaseEntity
{
    public Guid ProductId { get; set; }

    public Product Product { get; set; }

    public Guid PurchaseId { get; set; }

    public Purchase Purchase { get; set; }

    public Guid VolumeId { get; set; }

    public Volume Volume { get; set; }

    public int Amount { get; set; }
}

