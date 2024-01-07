using KoreanSecrets.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoreanSecrets.Domain.Configuration;

public class DemandConfiguration : IEntityTypeConfiguration<Demand>
{
    public void Configure(EntityTypeBuilder<Demand> builder)
    {
        //builder.HasMany(t => t.CategoryDemands)
        //    .WithOne(t => t.Demand)
        //    .HasForeignKey(t => t.DemandId)
        //    .OnDelete(DeleteBehavior.Cascade);
    }
}
