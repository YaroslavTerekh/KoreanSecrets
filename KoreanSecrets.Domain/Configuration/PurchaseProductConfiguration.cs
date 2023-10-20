using KoreanSecrets.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoreanSecrets.Domain.Configuration;

public class PurchaseProductConfiguration : IEntityTypeConfiguration<PurchasedProduct>
{
    public void Configure(EntityTypeBuilder<PurchasedProduct> builder)
    {
        builder.HasOne(t => t.Product)
            .WithMany()
            .HasForeignKey(t => t.ProductId)
            .OnDelete(DeleteBehavior.ClientSetNull);

        //TODO: fix
        builder.HasOne(t => t.Bucket)
            .WithMany(t => t.PurchaseProducts)
            .HasForeignKey(t => t.BucketId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
