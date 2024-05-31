using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoreanSecrets.Domain.Entities;

public class ProductDemand
{
    public Guid Id { get; set; }

    public Guid ProductId { get; set; }

    public Product Product { get; set; }

    public Guid DemandId { get; set; }

    public Demand Demand { get; set; }
}
