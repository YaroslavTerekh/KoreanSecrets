using KoreanSecrets.Domain.Common.Constants;
using KoreanSecrets.Domain.DataTransferObjects;
using KoreanSecrets.Domain.DbConnection;
using KoreanSecrets.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoreanSecrets.BL.Behaviors.Admin.Charts.GetYearUserChartStatistic;

public class GetYearUserChartStatisticHandler : IRequestHandler<GetYearUserChartStatisticQuery, ChartDTO>
{
    private readonly DataContext _context;
    private readonly UserManager<User> _userManager;

    public GetYearUserChartStatisticHandler(DataContext context, UserManager<User> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task<ChartDTO> Handle(GetYearUserChartStatisticQuery request, CancellationToken cancellationToken)
    {
        var users = await _context.Users.Where(t => t.CreatedTime >= DateTime.UtcNow.AddMonths(-12)).ToListAsync(cancellationToken);
        var admins = users.Where(t => _userManager.IsInRoleAsync(t, Roles.Admin).GetAwaiter().GetResult()).ToList();
        var total = users.Where(t => !admins.Contains(t)).Count();

        return new ChartDTO
        {
            TotalResult = total.ToString(),
            ChartInfo = new
            {
                Month12 = await GetValue(12),
                Month11 = await GetValue(11),
                Month10 = await GetValue(10),
                Month9 = await GetValue(9),
                Month8 = await GetValue(8),
                Month7 = await GetValue(7),
                Month6 = await GetValue(6),
                Month5 = await GetValue(5),
                Month4 = await GetValue(4),
                Month3 = await GetValue(3),
                Month2 = await GetValue(2),
                Month1 = await GetValue(1),
            }
        };
    }

    private async Task<int> GetValue(int val)
    {
        var users = await _context.Users
            .Where(t => t.CreatedTime.Month == val && t.CreatedTime.Year == DateTime.UtcNow.Year)
            .ToListAsync();
        var admins = users.Where(t => _userManager.IsInRoleAsync(t, Roles.Admin).GetAwaiter().GetResult()).ToList();
        var total = users.Where(t => !admins.Contains(t)).Count();

        return total;
    }
}
