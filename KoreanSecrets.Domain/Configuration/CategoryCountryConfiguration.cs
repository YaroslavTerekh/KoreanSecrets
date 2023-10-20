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

public class CategoryCountryConfiguration : IEntityTypeConfiguration<CategoryCountry>
{
    public void Configure(EntityTypeBuilder<CategoryCountry> builder)
    {
        builder.HasKey(cb => new { cb.CategoryId, cb.CountryId });

        builder.HasOne(cb => cb.Category)
            .WithMany(c => c.CategoryCountries)
            .HasForeignKey(cb => cb.CategoryId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(cb => cb.Country)
            .WithMany(b => b.CategoryCountries)
            .HasForeignKey(cb => cb.CountryId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
