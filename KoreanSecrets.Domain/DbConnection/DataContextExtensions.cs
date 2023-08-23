using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoreanSecrets.Domain.DbConnection;

public static class DataContextExtensions
{
    public static IServiceCollection AddDbContextsCustom(this IServiceCollection services, IConfiguration builder)
    {
        services.AddDbContext<DataContext>(
            o => o.UseSqlServer(builder.GetConnectionString("DefaultConnection"), b => b.MigrationsAssembly("KoreanSecrets.API")));
        return services;
    }
}
