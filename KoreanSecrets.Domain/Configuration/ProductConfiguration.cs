using KoreanSecrets.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoreanSecrets.Domain.Configuration;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.HasOne(t => t.Brand)
            .WithMany(t => t.Products)
            .HasForeignKey(t => t.BrandId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(t => t.SubCategory)
            .WithMany(t => t.Products)
            .HasForeignKey(t => t.SubCategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(t => t.Category)
            .WithMany(t => t.Products)
            .HasForeignKey(t => t.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(t => t.Country)
            .WithMany(t => t.Products)
            .HasForeignKey(t => t.CountryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(t => t.Demand)
            .WithMany(t => t.Products)
            .HasForeignKey(t => t.DemandId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(t => t.Guide)
            .WithOne(t => t.Product)
            .HasForeignKey<AppFile>(t => t.ProductId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(t => t.Feedbacks)
            .WithOne(t => t.Product)
            .HasForeignKey(t => t.ProductId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(t => t.Photos)
            .WithOne(t => t.ProductPhoto)
            .HasForeignKey(t => t.ProductPhotoId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
