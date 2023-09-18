using KoreanSecrets.BL.Services.Abstractions;
using KoreanSecrets.Domain.Common.Settings;
using KoreanSecrets.Domain.Models.NovaPost;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace KoreanSecrets.BL.Services.Realizations;

public class NovaPostService : INovaPostService
{
    private const string apiUrl = "https://api.novaposhta.ua/v2.0/json/";
    private readonly NovaPostConfiguration _configuration;

    public NovaPostService(NovaPostConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    public async Task<object> GetAllCitiesAsync(string cityName)
    {
        using var httpClient = new HttpClient();
        var response = await httpClient.PostAsync(apiUrl, new StringContent(JsonSerializer.Serialize(new NovaPostRequest(_configuration.ApiKey, "Address", "getSettlements")
        {
            methodProperties = new MethodProperties
            {
                FindByString = cityName,
                Limit = "10"
            }
        })));

        var responseData = JsonSerializer.Deserialize<object>(await response.Content.ReadAsStringAsync());

        return responseData;
    }

    public async Task<object> GetWarehousesAsync(string cityName, string warehouseName)
    {
        using var httpClient = new HttpClient();
        var response = await httpClient.PostAsync(apiUrl, new StringContent(JsonSerializer.Serialize(new NovaPostRequest(_configuration.ApiKey, "Address", "getWarehouses")
        {
            methodProperties = new MethodProperties
            {
                CityName = cityName,
                FindByString = warehouseName,
                Limit = "10"
            }
        })));

        var responseData = JsonSerializer.Deserialize<object>(await response.Content.ReadAsStringAsync());

        return responseData;
    }
}
