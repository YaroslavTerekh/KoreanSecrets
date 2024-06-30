using KoreanSecrets.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoreanSecrets.Domain.Configuration;

public class FeedbackConfiguration : IEntityTypeConfiguration<FeedbackReply>
{
    public void Configure(EntityTypeBuilder<FeedbackReply> builder)
    {
        builder.HasOne(t => t.Feedback)
            .WithMany(t => t.Replies)
            .HasForeignKey(t => t.FeedbackId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
