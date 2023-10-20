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

    public List<CategoryBrand> CategoryBrands { get; set; } = new();

    public List<CategoryCountry> CategoryCountries { get; set; } = new();

    public List<CategoryDemand> CategoryDemands { get; set; } = new();
    
    public List<CategorySubCategory> CategorySubCategories { get; set; } = new();
}
