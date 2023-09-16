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

var builder = WebApplication.CreateBuilder(args);

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

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddValidatorsFromAssembly(AppDomain.CurrentDomain.GetAssemblies().Where(t => t.FullName.Contains("BL")).First());
builder.Services.AddMediatR(AppDomain.CurrentDomain.GetAssemblies());

var emailConfig = builder.Configuration
        .GetSection("EmailConfiguration")
        .Get<EmailConfiguration>();
var novaPostConfig = builder.Configuration
        .GetSection("NovaPostConfiguration")
        .Get<NovaPostConfiguration>();
builder.Services.AddSingleton(emailConfig);
builder.Services.AddSingleton(novaPostConfig);
builder.Services.InjectServices();

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

var app = builder.Build();
var scope = app.Services.CreateScope();

var dataContext = scope.ServiceProvider.GetRequiredService<DataContext>();
dataContext.Database.Migrate();
await builder.Services.SeedDatabase(scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>(),
    scope.ServiceProvider.GetRequiredService<UserManager<User>>());

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCustomExceptionHandler();
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
