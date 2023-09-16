using KoreanSecrets.BL.Services.Abstractions;
using KoreanSecrets.Domain.Common.Settings;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace KoreanSecrets.BL.Behaviors.NovaPost.GetAllCities;

public class GetAllCitiesHandler : IRequestHandler<GetAllCitiesQuery, object>
{
    private readonly INovaPostService _novaPostService;

    public GetAllCitiesHandler(INovaPostService novaPostService)
    {
        _novaPostService = novaPostService;
    }

    public async Task<object> Handle(GetAllCitiesQuery request, CancellationToken cancellationToken)
    {

        return await _novaPostService.GetAllCitiesAsync(request.CityName);
    }
}
