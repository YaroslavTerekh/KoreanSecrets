using KoreanSecrets.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoreanSecrets.Domain.Configuration;

public class SubCategoryConfiguration : IEntityTypeConfiguration<SubCategory>
{
    public void Configure(EntityTypeBuilder<SubCategory> builder)
    {
        //builder.HasMany(t => t.CategorySubCategories)
        //    .WithOne(t => t.SubCategory)
        //    .HasForeignKey(t => t.SubCategoryId)
        //    .OnDelete(DeleteBehavior.Cascade);
    }
}
