using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoreanSecrets.BL.Behaviors.Admin.Brands.DeleteBrand;

public class DeleteBrandCommand : IRequest
{
    public Guid BrandId { get; set; }

    public DeleteBrandCommand(Guid id) => BrandId = id;
}
