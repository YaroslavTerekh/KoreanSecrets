using KoreanSecrets.BL.Behaviors;
using KoreanSecrets.Domain.Common.Constants;
using KoreanSecrets.Domain.Common.Enums;
using KoreanSecrets.Domain.DbConnection;
using KoreanSecrets.Domain.Entities;
using KoreanSecrets.Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using FluentValidation;
using KoreanSecrets.Domain.Common.DbSeed;
using KoreanSecrets.BL.Services;
using Microsoft.EntityFrameworkCore;
using KoreanSecrets.API.Middleware;
using System.Text.Json.Serialization;
using AutoMapper;
using KoreanSecrets.BL.Helpers;
using KoreanSecrets.Domain.Common.Settings;
using Hangfire;
using Newtonsoft.Json;
using Microsoft.Extensions.FileProviders;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<IISServerOptions>(options =>
{
    options.MaxRequestBodySize = int.MaxValue;
});

builder.Services.Configure<KestrelServerOptions>(options =>
{
    options.Limits.MaxRequestBodySize = int.MaxValue; // if don't set default value is: 30 MB
});

builder.Services.Configure<FormOptions>(x =>
{
    x.ValueLengthLimit = int.MaxValue;
    x.MultipartBodyLengthLimit = int.MaxValue; // if don't set default value is: 128 MB
    x.MultipartHeadersLengthLimit = int.MaxValue;
});

// Add services to the container.
builder.Services.AddDbContextsCustom(builder.Configuration);

builder.Services.AddIdentity<User, ApplicationRole>(opts =>
    {
        opts.User.RequireUniqueEmail = true;
    })
    .AddRoles<ApplicationRole>()
    .AddEntityFrameworkStores<DataContext>()
    .AddDefaultTokenProviders()
    .AddErrorDescriber<CustomIdentityErrorDescriber>();

builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 5;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = false;
});

builder.Services.AddValidatorsFromAssembly(AppDomain.CurrentDomain.GetAssemblies().Where(t => t.FullName.Contains("BL")).First());
builder.Services.AddMediatR(AppDomain.CurrentDomain.GetAssemblies());

var emailConfig = builder.Configuration
        .GetSection("EmailConfiguration")
        .Get<EmailConfiguration>();
var novaPostConfig = builder.Configuration
        .GetSection("NovaPostConfiguration")
        .Get<NovaPostConfiguration>();
var liqPayConfig = builder.Configuration
        .GetSection("LiqPaySettings")
        .Get<LiqPaySettings>();
var hostConfig = builder.Configuration
        .GetSection("HostSettings")
        .Get<HostSettings>();
var firebaseConfig = builder.Configuration
        .GetSection("FirebaseSettings")
        .Get<FirebaseSettings>();
var twillioConfig = builder.Configuration
        .GetRequiredSection("TwillioSettings")
        .Get<TwillioSettings>();
builder.Services.AddSingleton(twillioConfig);
builder.Services.AddSingleton(hostConfig);
builder.Services.AddSingleton(firebaseConfig);
builder.Services.AddSingleton(liqPayConfig);
builder.Services.AddSingleton(emailConfig);
builder.Services.AddSingleton(novaPostConfig);
builder.Services.InjectServices();

builder.Services.AddHangfire((sp, config) =>
{
    config.UseSqlServerStorage(builder.Configuration.GetConnectionString("DefaultConnection"));
    config.UseSerializerSettings(new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
}
);
builder.Services.AddHangfireServer();

builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 4;
    options.Password.RequiredUniqueChars = 0;

    options.User.RequireUniqueEmail = false;
});

builder.Services.AddSingleton(provider => new MapperConfiguration(cfg =>
{
    cfg.AddProfile(new MapperGlobalProfile(provider.GetRequiredService<HostSettings>()));
}).CreateMapper());

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "KoreanSecrets",
        Version = "v1"
    });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Example: \"Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header
            },
            new List<string>()
        }
    });
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ClockSkew = TimeSpan.Zero,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8
                .GetBytes(builder.Configuration.GetSection("AppSettings:Token").Value)),
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true
        };
        options.Events = new JwtBearerEvents
        {
            OnChallenge = async context =>
            {
                context.HandleResponse();

                var httpContext = context.HttpContext;
                var statusCode = StatusCodes.Status401Unauthorized;

                var routeData = httpContext.GetRouteData();
                var actionContext = new ActionContext(httpContext, routeData, new ActionDescriptor());

                var factory = httpContext.RequestServices.GetRequiredService<ProblemDetailsFactory>();
                var problemDetails = factory.CreateProblemDetails(httpContext, statusCode);

                var result = new ObjectResult(problemDetails) { StatusCode = statusCode };
                await result.ExecuteResultAsync(actionContext);
            },
            OnForbidden = async context =>
            {
                var httpContext = context.HttpContext;
                var statusCode = StatusCodes.Status403Forbidden;

                var routeData = httpContext.GetRouteData();
                var actionContext = new ActionContext(httpContext, routeData, new ActionDescriptor());

                var factory = httpContext.RequestServices.GetRequiredService<ProblemDetailsFactory>();
                var problemDetails = factory.CreateProblemDetails(httpContext, statusCode);

                var result = new ObjectResult(problemDetails) { StatusCode = StatusCodes.Status403Forbidden };
                await result.ExecuteResultAsync(actionContext);
            }
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(AuthPolicies.Admins, policy =>
    {
        policy.RequireRole(Roles.Admin);
    });

    options.AddPolicy(AuthPolicies.Users, policy =>
    {
        policy.RequireRole(Roles.User);
    });
});

builder.Services.AddCors(options => options.AddPolicy(
            "CORS",
            builder => builder
                .SetIsOriginAllowed(_ => true)
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials()));

var app = builder.Build();
var scope = app.Services.CreateScope();

var path = Path.Combine(app.Services.GetRequiredService<IWebHostEnvironment>().ContentRootPath, "uploads");
Directory.CreateDirectory(path);

app.UseFileServer(new FileServerOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "uploads")),
    RequestPath = "/uploads",
    EnableDefaultFiles = true
});

var dataContext = scope.ServiceProvider.GetRequiredService<DataContext>();
dataContext.Database.Migrate();
await builder.Services.SeedDatabase(scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>(),
    scope.ServiceProvider.GetRequiredService<UserManager<User>>());

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI();
//}

app.UseCustomExceptionHandler();
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseCors("CORS");

app.UseHangfireDashboard();
app.MapControllers();

app.Run();
