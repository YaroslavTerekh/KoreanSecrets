using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoreanSecrets.BL.Behaviors.Admin.AddCountry;

public class AddCountryCommand : IRequest
{
    public Guid CategoryId { get; set; }

    public string Title { get; set; }
}