using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoreanSecrets.Domain.Entities;

public class Category : BaseEntity
{
    public string Title { get; set; }

    public List<Product> Products { get; set; } = new();

    public List<Brand> Brands { get; set; } = new();

    public List<Country> Countries { get; set; } = new();

    public List<Demand> Demands { get; set; } = new();
    
    public List<SubCategory> SubCategories { get; set; } = new();
}
