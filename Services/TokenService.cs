using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using MazErpBack.Dtos.Users;
using MazErpBack.Services.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace MazErpBack.Services;

public class TokenService(IConfiguration config) : ITokenService
{
    private readonly string _jwtSecret = Environment.GetEnvironmentVariable("JWT_SECRET") ?? config["JWT_SECRET"] ??
    throw new ArgumentNullException("JWT_SECRET not found in environment variables or configuration.");

    public TokenDto CreateTokenAsync(User user)
    {
        try
        {
            var securityKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_jwtSecret));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Name),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim("userId", user.Id.ToString()),
                new Claim(ClaimTypes.Role, "Client")
            };
            if (user.UserWorkflows != null)
            {
                foreach (var cw in user.UserWorkflows)
                {
                    claims = claims.Append(new Claim(ClaimTypes.Role, cw.Role.ToString())).ToArray();
                }
            }
            var token = new JwtSecurityToken(
            "http://localhost",
            "*",
            claims,
            expires: DateTime.Now.AddMinutes(35),
            signingCredentials: credentials);
            var tokenResponse = new TokenDto
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = token.ValidTo
            };
            return tokenResponse;
        }
        catch (Exception)
        {
            // TODO
            throw;
        }
    }
}
