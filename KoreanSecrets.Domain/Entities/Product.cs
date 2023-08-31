using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoreanSecrets.Domain.Entities;

public class Product : BaseEntity
{
    public string Title { get; set; }

    public Guid BrandId { get; set; }

    public Brand Brand { get; set; }

    public Guid CategoryId { get; set; }

    public Category Category { get; set; }

    public Guid SubCategoryId { get; set; }

    public SubCategory SubCategory { get; set; }

    public Guid CountryId { get; set; }

    public Country Country { get; set; }

    public Guid DemandId { get; set; }

    public Demand Demand { get; set; }

    public string Volume { get; set; }

    public long Price { get; set; }

    public string Characteristics { get; set; }

    public string Usage { get; set; }

    public string Syllabes { get; set; }

    public Guid? GuideId { get; set; }

    public AppFile Guide { get; set; }

    public List<Feedback> Feedbacks { get; set; } = new();

    public List<Bucket> Buckets { get; set; } = new();
}
