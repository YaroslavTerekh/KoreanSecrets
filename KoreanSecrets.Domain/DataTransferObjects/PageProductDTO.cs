using KoreanSecrets.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoreanSecrets.Domain.DataTransferObjects;

public class PageProductDTO : BaseEntity
{
    public string Title { get; set; }

    public Guid BrandId { get; set; }

    public BrandDTO Brand { get; set; }

    public Guid CategoryId { get; set; }

    public Guid SubCategoryId { get; set; }

    public SubCategoryDTO SubCategory { get; set; }

    public Guid CountryId { get; set; }

    public CountryDTO Country { get; set; }

    public Guid DemandId { get; set; }

    public DemandDTO Demand { get; set; }

    public string Volume { get; set; }

    public long Price { get; set; }

    public string Characteristics { get; set; }

    public string Usage { get; set; }

    public string Syllabes { get; set; }

    public Guid? GuideId { get; set; }

    public AppFileDTO Guide { get; set; }

    public List<AppFileDTO> Photos { get; set; }
    
    public List<FeedbackDTO> Feedbacks { get; set; } = new();

    public List<ListProductDTO> SameProducts { get; set; } = new();
}
