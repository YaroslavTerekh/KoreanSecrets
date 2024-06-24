using KoreanSecrets.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoreanSecrets.Domain.Configuration;

public class VolumeConfiguration : IEntityTypeConfiguration<Volume>
{
    public void Configure(EntityTypeBuilder<Volume> builder)
    {
        builder.HasMany(t => t.Photos)
            .WithOne(t => t.VolumePhoto)
            .HasForeignKey(t => t.VolumePhotoId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
