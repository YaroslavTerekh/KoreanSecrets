using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoreanSecrets.Domain.Entities;

public class CategorySubCategory
{
    public Guid Id { get; set; }

    public Guid CategoryId { get; set; }

    public Category Category { get; set; }

    public Guid SubCategoryId { get; set; }

    public SubCategory SubCategory { get; set; }
}
