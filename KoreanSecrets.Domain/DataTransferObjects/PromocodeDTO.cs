using KoreanSecrets.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoreanSecrets.Domain.DataTransferObjects;

public class PromocodeDTO : BaseEntity
{
    public string Code { get; set; }

    public decimal Discount { get; set; }

    public bool IsActive { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public Guid? BrandId { get; set; }

    public BrandDTO Brand { get; set; }
}
