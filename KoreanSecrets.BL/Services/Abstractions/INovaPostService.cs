using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoreanSecrets.BL.Services.Abstractions;

public interface INovaPostService
{
    public Task<object> GetAllCitiesAsync(string cityName);

    public Task<object> GetWarehousesAsync(string cityName, string warehouseName);
}
