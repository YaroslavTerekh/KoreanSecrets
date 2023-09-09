using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoreanSecrets.BL.Behaviors.Admin.Countries.DeleteCountry;

public class DeleteCountryCommand : IRequest
{
    public Guid CountryId { get; set; }

    public DeleteCountryCommand(Guid id) => CountryId = id;
}
