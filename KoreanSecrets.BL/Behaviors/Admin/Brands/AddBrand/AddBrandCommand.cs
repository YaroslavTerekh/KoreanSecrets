using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoreanSecrets.BL.Behaviors.Admin.Brands.AddBrand;

public class AddBrandCommand : IRequest
{
    public Guid CategoryId { get; set; }

    public string Title { get; set; }

    public IFormFile Photo { get; set; }
}
