using KoreanSecrets.Domain.DataTransferObjects;
using KoreanSecrets.Domain.DbConnection;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoreanSecrets.BL.Behaviors.Admin.Charts.GetYearMoneyStatistic;

public class GetYearMoneyStatisticHandler : IRequestHandler<GetYearMoneyStatisticQuery, ChartDTO>
{
    private readonly DataContext _context;

    public GetYearMoneyStatisticHandler(DataContext context)
    {
        _context = context;
    }

    public async Task<ChartDTO> Handle(GetYearMoneyStatisticQuery request, CancellationToken cancellationToken)
    {
        var total = await _context.Purchases.Where(t => t.CreatedDate >= DateTime.UtcNow.AddMonths(-12)).Select(t => t.TotalPrice).SumAsync(cancellationToken);

        return new ChartDTO
        {
            TotalResult = total.ToString() + " грн",
            ChartInfo = new
            {
                Month12 = await GetValue(0),
                Month11 = await GetValue(-1),
                Month10 = await GetValue(-2),
                Month9 = await GetValue(-3),
                Month8 = await GetValue(-4),
                Month7 = await GetValue(-5),
                Month6 = await GetValue(-6),
                Month5 = await GetValue(-7),
                Month4 = await GetValue(-8),
                Month3 = await GetValue(-9),
                Month2 = await GetValue(-10),
                Month1 = await GetValue(-11),
            }
        };
    }

    private async Task<long> GetValue(int val)
    {
        return await _context.Purchases.Where(t => t.CreatedDate.Month == DateTime.UtcNow.AddMonths(val).Month
            && t.CreatedDate.Year == DateTime.UtcNow.AddMonths(val).Year
        ).Select(t => t.TotalPrice).SumAsync();
    }
}
