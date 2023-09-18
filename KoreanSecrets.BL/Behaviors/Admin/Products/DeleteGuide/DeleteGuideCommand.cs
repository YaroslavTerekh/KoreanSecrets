using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoreanSecrets.BL.Behaviors.Admin.Products.DeleteGuide;

public class DeleteGuideCommand : IRequest
{
    public Guid ProductId { get; set; }

    public Guid FileId { get; set; }

    public DeleteGuideCommand(Guid productId, Guid fileId)
    {
        ProductId = productId;
        FileId = fileId;
    }
}
