using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using MazErpBack.Dtos.Login;
using MazErpBack.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace MazErpBack.Services.User;

public class TokenService(IConfiguration config) : ITokenService
{
    private readonly string _jwtSecret = Environment.GetEnvironmentVariable("JWT_SECRET") ?? config["JWT_SECRET"] ??
    throw new ArgumentNullException("JWT_SECRET not found in environment variables or configuration.");

    public TokenDto CreateTokenAsync(Client client)
    {
        try
        {
            var securityKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_jwtSecret));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, client.Name),
                new Claim(ClaimTypes.Email, client.Email),
                new Claim("userId", client.Id.ToString()),
                new Claim(ClaimTypes.Role, "Client")
            };
            if (client.ClientWorkflows != null)
            {
                foreach (var cw in client.ClientWorkflows)
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
