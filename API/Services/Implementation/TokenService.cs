using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using MazErpAPI.DTOs.Users;
using MazErpAPI.Models;
using MazErpAPI.Services.Interfaces;
using MazErpBack.DTOs.Users;
using Microsoft.IdentityModel.Tokens;

namespace MazErpBack.Services.Implementation;

public class TokenService(IConfiguration config) : ITokenService
{
    private readonly string _jwtSecret = Environment.GetEnvironmentVariable("JWT_SECRET")
        ?? config["JWT_SECRET"]
        ?? throw new ArgumentNullException("JWT_SECRET not found in environment variables or configuration.");

    private readonly string _issuer = config["Jwt:Issuer"] ?? "MazErpBack";
    private readonly string _audience = config["Jwt:Audience"] ?? "MazErpFront";

    public TokenDto CreateToken(User user)
    {
        try
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSecret));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new(ClaimTypes.Role, "user")
            };

            var token = new JwtSecurityToken(
                issuer: _issuer,
                audience: _audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(35),
                signingCredentials: credentials);

            var userDto = new UserDto
            {
                Id = user.Id,
                Email = user.Email,
                Name = user.Name,
                ProfilePhotoUrl = user.ProfilePhotoUrl
            };

            var tokenResponse = new TokenDto
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = token.ValidTo,
                UserDto = userDto
            };

            return tokenResponse;
        }
        catch (Exception ex)
        {
            throw new ApplicationException($"Error creating token for user {user.Email}", ex);
        }
    }
}