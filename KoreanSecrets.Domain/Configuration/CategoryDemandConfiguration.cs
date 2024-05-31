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

public class CategoryDemandConfiguration : IEntityTypeConfiguration<ProductDemand>
{
    public void Configure(EntityTypeBuilder<ProductDemand> builder)
    {
        builder.HasKey(cb => new { cb.ProductId, cb.DemandId });

        builder
            .HasOne(cb => cb.Product)
            .WithMany(c => c.ProductDemands)
            .HasForeignKey(cb => cb.ProductId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(cb => cb.Demand)
            .WithMany(b => b.ProductDemands)
            .HasForeignKey(cb => cb.DemandId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
