using KoreanSecrets.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoreanSecrets.Domain.Configuration;

public class BrandConfiguration : IEntityTypeConfiguration<Brand>
{
    public void Configure(EntityTypeBuilder<Brand> builder)
    {
        builder.HasOne(t => t.Photo)
            .WithOne(t => t.BrandPhoto)
            .HasForeignKey<AppFile>(t => t.BrandPhotoId)
            .OnDelete(DeleteBehavior.NoAction);

        //builder.HasMany(t => t.CategoryBrands)
        //    .WithOne(t => t.Brand)
        //    .HasForeignKey(t => t.BrandId)
        //    .OnDelete(DeleteBehavior.Cascade);
    }
}
