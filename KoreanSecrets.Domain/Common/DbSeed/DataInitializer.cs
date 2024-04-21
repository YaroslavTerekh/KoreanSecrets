using KoreanSecrets.Domain.Common.Constants;
using KoreanSecrets.Domain.Entities;
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
    public async static Task<IServiceCollection> SeedDatabase(
        this IServiceCollection services, 
        RoleManager<ApplicationRole> roleManager,
        UserManager<User> userManager
    )
    {
        if(!roleManager.Roles.AnyAsync(t => t.Name == Roles.Admin).GetAwaiter().GetResult())
            roleManager.CreateAsync(new ApplicationRole(Roles.Admin)).GetAwaiter().GetResult();

        if(!roleManager.Roles.AnyAsync(t => t.Name == Roles.User).GetAwaiter().GetResult())
            roleManager.CreateAsync(new ApplicationRole(Roles.User)).GetAwaiter().GetResult();

        var admins = await userManager.GetUsersInRoleAsync(Roles.Admin);
        var phoneNumbers = admins.Select(t => t.PhoneNumber).ToList();

        if (!phoneNumbers.Contains("+380999999999"))
        {
            var admin = new User
            {
                UserName = "Admin",
                FirstName = "Admin",
                LastName = "Admin",
                PhoneNumber = "+380999999999",
                Email = "Admin@gmail.com",
                PhoneNumberConfirmed = true
            };

            await userManager.CreateAsync(admin, "Pa$$word123!");
            await userManager.AddToRoleAsync(admin, Roles.Admin);

        }

        if (!phoneNumbers.Contains("+380666666666"))
        {
            var admin2 = new User
            {
                UserName = "Nightmare",
                FirstName = "Nightmare",
                LastName = "Nightmare",
                PhoneNumber = "+380666666666",
                Email = "Nightmarea@gmail.com",
                PhoneNumberConfirmed = true
            };

            await userManager.CreateAsync(admin2, "Pa$$word123!");
            await userManager.AddToRoleAsync(admin2, Roles.Admin);
        }

        return services;
    }
}
