using KoreanSecrets.Domain.DataTransferObjects;
using KoreanSecrets.Domain.DbConnection;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace KoreanSecrets.BL.Behaviors.Admin.Charts.GetDayMoneyStatistic;

public class GetDayMoneyStatisticHandler : IRequestHandler<GetDayMoneyStatisticQuery, ChartDTO>
{
    private readonly DataContext _context;

    public GetDayMoneyStatisticHandler(DataContext context)
    {
        _context = context;
    }

    public async Task<ChartDTO> Handle(GetDayMoneyStatisticQuery request, CancellationToken cancellationToken)
    {
        var total = await _context.Purchases.Where(t => t.CreatedDate >= DateTime.UtcNow.AddHours(-12)).Select(t => t.TotalPrice).SumAsync(cancellationToken);

        return new ChartDTO
        {
            TotalResult = total.ToString() + " грн",
            ChartInfo = new
            {
                Hour12 = await GetValue(0),
                Hour11 = await GetValue(-1),
                Hour10 = await GetValue(-2),
                Hour9 = await GetValue(-3),
                Hour8 = await GetValue(-4),
                Hour7 = await GetValue(-5),
                Hour6 = await GetValue(-6),
                Hour5 = await GetValue(-7),
                Hour4 = await GetValue(-8),
                Hour3 = await GetValue(-9),
                Hour2 = await GetValue(-10),
                Hour1 = await GetValue(-11),
            }
        };
    }

    private async Task<long> GetValue(int val)
    {
        return await _context.Purchases.Where(t => t.CreatedDate.Hour == DateTime.UtcNow.AddHours(val).Hour
            && t.CreatedDate.DayOfYear == DateTime.UtcNow.AddHours(val).DayOfYear
            && t.CreatedDate.Year == DateTime.UtcNow.AddHours(val).Year
        ).Select(t => t.TotalPrice).SumAsync();
    }
}
