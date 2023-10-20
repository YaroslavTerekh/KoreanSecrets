using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoreanSecrets.Domain.Entities;

public class Demand : BaseEntity
{
    public string Title { get; set; }

    public List<Product> Products { get; set; } = new();

    public List<CategoryDemand> CategoryDemands { get; set; } = new();
}
