using KoreanSecrets.Domain.DataTransferObjects;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoreanSecrets.BL.Behaviors.Products.GetCountriesWithoutPagination;

public class GetCountriesWithoutPaginationQuery : IRequest<List<CountryDTO>>
{
}
