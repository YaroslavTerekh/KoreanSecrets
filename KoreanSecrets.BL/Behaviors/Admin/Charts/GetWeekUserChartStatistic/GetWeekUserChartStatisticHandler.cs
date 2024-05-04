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
using System.Threading;
using System.Threading.Tasks;

namespace KoreanSecrets.BL.Behaviors.Admin.Charts.GetWeekUserChartStatistic;

public class GetWeekUserChartStatisticHandler : IRequestHandler<GetWeekUserChartStatisticQuery, ChartDTO>
{
    private readonly DataContext _context;
    private readonly UserManager<User> _userManager;

    public GetWeekUserChartStatisticHandler(DataContext context, UserManager<User> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task<ChartDTO> Handle(GetWeekUserChartStatisticQuery request, CancellationToken cancellationToken)
    {
        var users = await _context.Users.Where(t => t.CreatedTime >= DateTime.UtcNow.AddDays(-7)).ToListAsync(cancellationToken);
        var admins = users.Where(t => _userManager.IsInRoleAsync(t, Roles.Admin).GetAwaiter().GetResult()).ToList();
        var total = users.Where(t => !admins.Contains(t)).Count();

        return new ChartDTO
        {
            TotalResult = total.ToString(),
            ChartInfo = new
            {
                Day7 = await GetValue(0),
                Day6 = await GetValue(-1),
                Day5 = await GetValue(-2),
                Day4 = await GetValue(-3),
                Day3 = await GetValue(-4),
                Day2 = await GetValue(-5),
                Day1 = await GetValue(-6),
            }
        };
    }

    private async Task<int> GetValue(int val)
    {
        var users = await _context.Users
            .Where(t => t.CreatedTime.DayOfYear == DateTime.UtcNow.AddDays(val).DayOfYear && t.CreatedTime.Year == DateTime.UtcNow.AddDays(val).Year)
            .ToListAsync();
        var admins = users.Where(t => _userManager.IsInRoleAsync(t, Roles.Admin).GetAwaiter().GetResult()).ToList();
        var total = users.Where(t => !admins.Contains(t)).Count();

        return total;
    }
}
