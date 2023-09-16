using KoreanSecrets.BL.Services.Abstractions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoreanSecrets.BL.Behaviors.NovaPost.GetWarehouses;

public class GetWarehousesHandler : IRequestHandler<GetWarehousesQuery, object>
{
    private readonly INovaPostService _novaPostService;

    public GetWarehousesHandler(INovaPostService novaPostService)
    {
        _novaPostService = novaPostService;
    }

    public async Task<object> Handle(GetWarehousesQuery request, CancellationToken cancellationToken)
    {
        return await _novaPostService.GetWarehousesAsync(request.CityName, request.WarehouseName);
    }
}
