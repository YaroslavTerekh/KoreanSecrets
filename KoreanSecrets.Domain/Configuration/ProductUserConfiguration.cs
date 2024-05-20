using KoreanSecrets.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoreanSecrets.Domain.Configuration;

public class ProductUserConfiguration : IEntityTypeConfiguration<ProductUser>
{
    public void Configure(EntityTypeBuilder<ProductUser> builder)
    {
        builder.HasKey(u => new { u.LikesId, u.LikesId1 });

        builder.HasOne(uc => uc.Likes)
                .WithMany(u => u.Likes)
                .HasForeignKey(uc => uc.LikesId);

        builder.HasOne(uc => uc.Likes1)
                .WithMany(c => c.Likes)
                .HasForeignKey(uc => uc.LikesId1);
    }
}
