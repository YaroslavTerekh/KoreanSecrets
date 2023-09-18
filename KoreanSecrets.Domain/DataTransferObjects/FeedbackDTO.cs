using KoreanSecrets.Domain.Common.Enums;
using KoreanSecrets.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoreanSecrets.Domain.DataTransferObjects;

public class FeedbackDTO : BaseEntity
{
    public string FeedbackText { get; set; }

    public FeedbackRate Rate { get; set; }

    public UserDTO User { get; set; }
}
