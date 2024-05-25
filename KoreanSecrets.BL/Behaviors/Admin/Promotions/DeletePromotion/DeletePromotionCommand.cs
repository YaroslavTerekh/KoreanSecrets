using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoreanSecrets.BL.Behaviors.Admin.Promotions.DeletePromotion;

public class DeletePromotionCommand : IRequest
{
    public Guid PromoId { get; set; }

    public DeletePromotionCommand(Guid id) => PromoId = id;
}
