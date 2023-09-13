using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoreanSecrets.BL.Behaviors.Admin.Promocodes.DeletePromocode;

public class DeletePromocodeCommand : IRequest
{
    public Guid PromocodeId { get; set; }

    public DeletePromocodeCommand(Guid id) => PromocodeId = id;
}
