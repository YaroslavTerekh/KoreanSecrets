using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoreanSecrets.BL.Behaviors.Admin.Users.DeleteFeedback;

public class DeleteFeedbackCommand : IRequest
{
    public Guid Id { get; set; }

    public DeleteFeedbackCommand(Guid id) => Id = id;
}
