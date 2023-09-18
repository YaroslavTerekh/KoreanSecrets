using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoreanSecrets.BL.Behaviors.NovaPost.GetAllCities;

public class GetAllCitiesQuery : IRequest<object>
{
    public string CityName { get; set; }

    public GetAllCitiesQuery(string name) => CityName = name;
}
