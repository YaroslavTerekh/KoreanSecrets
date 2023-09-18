using KoreanSecrets.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoreanSecrets.Domain.Configuration;

public class BannerConfiguration : IEntityTypeConfiguration<Banner>
{
    public void Configure(EntityTypeBuilder<Banner> builder)
    {
        builder.HasOne(t => t.BannerPhoto)
            .WithOne(t => t.Banner)
            .HasForeignKey<AppFile>(t => t.BannerId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
