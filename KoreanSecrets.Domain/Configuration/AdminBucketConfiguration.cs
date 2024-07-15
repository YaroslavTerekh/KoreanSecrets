using KoreanSecrets.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoreanSecrets.Domain.Configuration;

public class AdminBucketConfiguration : IEntityTypeConfiguration<AdminBucket>
{
    public void Configure(EntityTypeBuilder<AdminBucket> builder)
    {
        builder.HasMany(t => t.BucketProducts)
            .WithOne(t => t.AdminBucket)
            .HasForeignKey(t => t.AdminBucketId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
