using KoreanSecrets.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoreanSecrets.Domain.DataTransferObjects;

public class FeedbackReplyDTO : BaseEntity
{
    public bool IsReply { get; set; } = true;

    public Guid FeedbackId { get; set; }

    public FeedbackDTO Feedback { get; set; }

    public string ReplyText { get; set; }

    public Guid UserId { get; set; }

    public UserDTO User { get; set; }
}
