using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoreanSecrets.Domain.Entities;

public class CategoryDemand
{
    public Guid Id { get; set; }

    public Guid CategoryId { get; set; }

    public Category Category { get; set; }

    public Guid DemandId { get; set; }

    public Demand Demand { get; set; }
}
