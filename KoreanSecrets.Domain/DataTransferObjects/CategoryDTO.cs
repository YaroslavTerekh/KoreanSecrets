using KoreanSecrets.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoreanSecrets.Domain.DataTransferObjects;

public class CategoryDTO : BaseEntity
{
    public string Title { get; set; }

    public List<ListProductDTO> Products { get; set; } = new();

    public List<BrandDTO> Brands { get; set; } = new();

    public List<CountryDTO> Countries { get; set; } = new();

    public List<DemandDTO> Demands { get; set; } = new();

    public List<SubCategoryDTO> SubCategories { get; set; } = new();
}
