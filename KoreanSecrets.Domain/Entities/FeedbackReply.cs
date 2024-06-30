using KoreanSecrets.Domain.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoreanSecrets.Domain.Entities;

public class FeedbackReply : BaseEntity
{
    public bool IsReply { get; set; } = true;

    public Guid FeedbackId { get; set; }

    public Feedback Feedback { get; set; }

    public string ReplyText { get; set; }

    public Guid UserId { get; set; }

    public User User { get; set; }
}
