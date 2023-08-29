using KoreanSecrets.Domain.Common.Constants;
using KoreanSecrets.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoreanSecrets.Domain.Common.DbSeed;

public static class DataInitializer
{
    public static IServiceCollection SeedDatabase(
        this IServiceCollection services, 
        RoleManager<ApplicationRole> roleManager
    )
    {
        if(!roleManager.Roles.AnyAsync(t => t.Name == Roles.Admin).GetAwaiter().GetResult())
            roleManager.CreateAsync(new ApplicationRole(Roles.Admin)).GetAwaiter().GetResult();

        if(!roleManager.Roles.AnyAsync(t => t.Name == Roles.User).GetAwaiter().GetResult())
            roleManager.CreateAsync(new ApplicationRole(Roles.User)).GetAwaiter().GetResult();
        return services;
    }
}
