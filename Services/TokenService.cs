using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DigitalProject.DB;
using DigitalProject.Settings;
using Microsoft.IdentityModel.Tokens;

namespace DigitalProject.Services;

public interface ITokenService {
    string BuildToken(Faculty user);
}

public class TokenService : ITokenService
{
    private IJwtSettings settings;

    public TokenService(IJwtSettings settings) {
        this.settings = settings;
    }

    public string BuildToken(Faculty user)
    {
        Claim[] claims = new Claim[] {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(settings.SecretKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            claims: claims, 
            expires: DateTime.Now.AddMinutes(settings.MinutesExpire),
            signingCredentials: creds
        );
        string tokenLiteral = new JwtSecurityTokenHandler().WriteToken(token);
        return "Bearer " + tokenLiteral;
    }
}