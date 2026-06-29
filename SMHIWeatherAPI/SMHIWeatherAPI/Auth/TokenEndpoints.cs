using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace SMHIWeatherAPI.Auth;

public static class TokenEndpoints
{
    public static void MapTokenEndpoints(this WebApplication app)
    {
        app.MapPost("/token", GenerateToken).AllowAnonymous();
    }

    private static IResult GenerateToken(TokenRequest request, IConfiguration config)
    {
        if (request.Password != config["Jwt:Password"])
            return Results.Unauthorized();

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]!));
        var token = new JwtSecurityToken(
            issuer: config["Jwt:Issuer"],
            audience: config["Jwt:Audience"],
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
        );

        return Results.Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token) });
    }
}

public record TokenRequest(string Password);
