using KoreanSecrets.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace KoreanSecrets.BL.Behaviors.UserSelf.ReplyFeedback;

public class ReplyFeedbackCommand : IRequest
{
    public Guid FeedbackId { get; set; }

    public string ReplyText { get; set; }

    [JsonIgnore]
    public Guid UserId { get; set; }
}
