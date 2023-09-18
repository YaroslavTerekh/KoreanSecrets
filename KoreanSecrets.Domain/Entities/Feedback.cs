using KoreanSecrets.Domain.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoreanSecrets.Domain.Entities;

public class Feedback : BaseEntity
{
    public string FeedbackText { get; set; }

    public FeedbackRate Rate { get; set; }

    public Guid UserId { get; set; }

    public User User { get; set; }

    public Guid ProductId { get; set; }

    public Product Product { get; set; }
}
