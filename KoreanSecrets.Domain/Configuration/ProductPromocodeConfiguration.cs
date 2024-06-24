using KoreanSecrets.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoreanSecrets.Domain.Configuration;

public class ProductPromocodeConfiguration : IEntityTypeConfiguration<ProductPromocode>
{
    public void Configure(EntityTypeBuilder<ProductPromocode> builder)
    {
        builder.HasKey(u => new { u.PromocodeId, u.ProductId });

        builder.HasOne(uc => uc.Promocode)
                .WithMany(u => u.Products)
                .HasForeignKey(uc => uc.PromocodeId);

        builder.HasOne(uc => uc.Product)
                .WithMany(c => c.Promocodes)
                .HasForeignKey(uc => uc.ProductId);
    }
}
