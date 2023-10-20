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

public class CategoryBrandConfiguration : IEntityTypeConfiguration<CategoryBrand>
{
    public void Configure(EntityTypeBuilder<CategoryBrand> builder)
    {
        builder.HasKey(cb => new { cb.CategoryId, cb.BrandId });

        builder.HasOne(cb => cb.Category)
            .WithMany(c => c.CategoryBrands)
            .HasForeignKey(cb => cb.CategoryId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(cb => cb.Brand)
            .WithMany(b => b.CategoryBrands)
            .HasForeignKey(cb => cb.BrandId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
