using KoreanSecrets.BL.Services.Abstractions;
using KoreanSecrets.Domain.Entities;
using KoreanSecrets.Domain.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace KoreanSecrets.BL.Services.Realizations;

public class JWTService : IJWTService
{
    private readonly IConfiguration _configuration;

    public JWTService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public AuthToken GenerateJWT(User user, string[] roles)
    {
        List<Claim> claims = new List<Claim>
            {
                new Claim(type: ClaimTypes.NameIdentifier, value: user.Id.ToString()),
                new Claim(type: ClaimTypes.Email, user.Email),                
            };

        foreach (var role in roles)
            claims.Add(new Claim(type: ClaimTypes.Role, role));

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value));

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expireDate = DateTime.UtcNow.AddDays(14);

        var token = new JwtSecurityToken(
            claims: claims,
            expires: expireDate,
            signingCredentials: creds);

        var jwt = new JwtSecurityTokenHandler().WriteToken(token);

        return new AuthToken(jwt, expireDate);
    }
}