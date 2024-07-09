using KoreanSecrets.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoreanSecrets.Domain.Configuration;

public class VolumeUserConfiguration : IEntityTypeConfiguration<VolumeUser>
{
    public void Configure(EntityTypeBuilder<VolumeUser> builder)
    {
        builder.HasKey(u => new { u.UserId, u.VolumeId });

        builder.HasOne(uc => uc.Volume)
                .WithMany(u => u.UsersWaitingForStock)
                .HasForeignKey(uc => uc.VolumeId);

        builder.HasOne(uc => uc.User)
                .WithMany(c => c.VolumesWaitingForStock)
                .HasForeignKey(uc => uc.UserId);
    }
}
