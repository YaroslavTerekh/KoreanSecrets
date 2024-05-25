using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoreanSecrets.BL.Behaviors.Admin.Promotions.ModifyPromotion;

public class ModifyPromotionCommand : IRequest
{
    public Guid Id { get; set; }

    public Guid BrandId { get; set; }

    public decimal Discount { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }
}
