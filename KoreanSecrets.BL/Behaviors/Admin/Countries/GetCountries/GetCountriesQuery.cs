using KoreanSecrets.Domain.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoreanSecrets.BL.Behaviors.Admin.Countries.GetCountries;

public class GetCountriesQuery : IAuthorizedRequest<List<CountryDTO>>
{
}
