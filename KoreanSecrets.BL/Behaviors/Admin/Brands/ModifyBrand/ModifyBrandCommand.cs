using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace KoreanSecrets.BL.Behaviors.Admin.Brands.ModifyBrand;

public class ModifyBrandCommand : IRequest
{
    [JsonIgnore]
    public Guid Id { get; set; }

    public string Title { get; set; }
}
