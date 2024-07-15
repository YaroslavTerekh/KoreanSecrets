using KoreanSecrets.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoreanSecrets.Domain.Configuration;

public class BucketProductConfiguration : IEntityTypeConfiguration<BucketProduct>
{
    public void Configure(EntityTypeBuilder<BucketProduct> builder)
    {
        builder.HasOne(t => t.Product)
            .WithMany()
            .HasForeignKey(t => t.ProductId)
            .OnDelete(DeleteBehavior.ClientSetNull);

        builder.HasOne(t => t.Volume)
            .WithMany()
            .HasForeignKey(t => t.VolumeId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(t => t.Bucket)
            .WithMany(t => t.BucketProducts)
            .HasForeignKey(t => t.BucketId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(t => t.AdminBucket)
            .WithMany(t => t.BucketProducts)
            .HasForeignKey(t => t.AdminBucketId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
