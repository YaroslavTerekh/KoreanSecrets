using KoreanSecrets.Domain.Common.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace KoreanSecrets.BL.Behaviors.UserSelf.AddFeedback;

public class AddFeedbackCommand : IRequest
{
    [JsonIgnore]
    public Guid CurrentUserId { get; set; }

    public Guid ProductId { get; set; }

    public string FeedbackText { get; set; }

    public FeedbackRate Rate { get; set; }
}
