﻿using KoreanSecrets.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoreanSecrets.Domain.Configuration;

public class PurchaseConfiguration : IEntityTypeConfiguration<Purchase>
{
    public void Configure(EntityTypeBuilder<Purchase> builder)
    {
        builder.Property(t => t.PurchaseIdentifier)
            .IsRequired(true);

        builder.HasOne(t => t.User)
            .WithMany(t => t.Purchases)
            .HasForeignKey(t => t.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(t => t.Products)
            .WithOne()
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(t => t.Promocode)
            .WithMany()
            .IsRequired(false)
            .HasForeignKey(t => t.PromocodeId)
            .OnDelete(DeleteBehavior.ClientSetNull);
    }
}
