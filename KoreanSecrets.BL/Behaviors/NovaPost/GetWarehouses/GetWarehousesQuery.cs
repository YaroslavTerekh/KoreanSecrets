using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoreanSecrets.BL.Behaviors.NovaPost.GetWarehouses;

public class GetWarehousesQuery : IRequest<object>
{
    public string CityName { get; set; }

    public string WarehouseName { get; set; }

    public GetWarehousesQuery(string cityName, string warehouseName)
    {
        CityName = cityName;
        WarehouseName = warehouseName;
    }
}
