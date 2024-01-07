using KoreanSecrets.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace KoreanSecrets.Domain.Configuration;

public class CategorySubCategoryConfiguration : IEntityTypeConfiguration<CategorySubCategory>
{
    public void Configure(EntityTypeBuilder<CategorySubCategory> builder)
    {
        //builder.HasKey(cb => new { cb.CategoryId, cb.SubCategoryId });

        //builder.HasOne(cb => cb.Category)
        //    .WithMany(c => c.CategorySubCategories)
        //    .HasForeignKey(cb => cb.CategoryId)
        //    .OnDelete(DeleteBehavior.Cascade);

        //builder.HasOne(cb => cb.SubCategory)
        //    .WithMany(b => b.CategorySubCategories)
        //    .HasForeignKey(cb => cb.SubCategoryId)
        //    .OnDelete(DeleteBehavior.Cascade);
    }
}
