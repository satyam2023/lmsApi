namespace lmsApi.Helper.Jwt;

using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;

public class JwtHelper
{
    private readonly IConfiguration _configuration;

    public JwtHelper(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string GenerateAccessToken(User user)
    {
        var jwtKey = _configuration["JwtSettings:Key"] ?? "your-secret-key-here-must-be-at-least-32-characters";
        var jwtIssuer = _configuration["JwtSettings:Issuer"] ?? "lmsApi";
        var jwtAudience = _configuration["JwtSettings:Audience"] ?? "lmsApi";
        var expiryHours = int.Parse(_configuration["JwtSettings:ExpiryInHours"] ?? "1");

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(jwtKey);

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Email, user.Email),
            new(ClaimTypes.Name, user.Name),
            new(ClaimTypes.Role, user.Role)
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddHours(expiryHours),
            Issuer = jwtIssuer,
            Audience = jwtAudience,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    public string GenerateRefreshToken()
    {
        return Guid.NewGuid().ToString();
    }

    public string ExtractUserNameFromToken(string token)
    {
        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);
        var email = jwtToken
            ?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email || c.Type == "email")
            ?.Value;

        return email ?? "";
    }
}