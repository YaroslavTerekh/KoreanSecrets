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

public class CategoryDemandConfiguration : IEntityTypeConfiguration<CategoryDemand>
{
    public void Configure(EntityTypeBuilder<CategoryDemand> builder)
    {
        //builder.HasKey(cb => new { cb.CategoryId, cb.DemandId });

        //builder
        //    .HasOne(cb => cb.Category)
        //    .WithMany(c => c.CategoryDemands)
        //    .HasForeignKey(cb => cb.CategoryId)
        //    .OnDelete(DeleteBehavior.Cascade);

        //builder.HasOne(cb => cb.Demand)
        //    .WithMany(b => b.CategoryDemands)
        //    .HasForeignKey(cb => cb.DemandId)
        //    .OnDelete(DeleteBehavior.Cascade);
    }
}
