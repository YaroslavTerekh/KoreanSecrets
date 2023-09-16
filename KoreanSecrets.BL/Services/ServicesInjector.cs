using KoreanSecrets.BL.Behaviors;
using KoreanSecrets.BL.Services.Abstractions;
using KoreanSecrets.BL.Services.Realizations;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoreanSecrets.BL.Services;

public static class ServicesInjector
{
    public static IServiceCollection InjectServices(this IServiceCollection services)
    {
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        services.AddTransient<IJWTService, JWTService>();
        services.AddTransient<IPhoneNumberService, PhoneNumberService>();
        services.AddTransient<IFileService, FileService>();
        services.AddTransient<IEmailService, EmailService>();
        services.AddTransient<INovaPostService, NovaPostService>();

        return services;
    }
}
