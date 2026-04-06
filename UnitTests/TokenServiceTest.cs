using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using MazErpAPI.Models;
using MazErpAPI.Services.Implementation;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Moq;

namespace UnitTests;

public class TokenServiceTest
{
    private const string ValidJwtSecret = "ThisIsAValidJwtSecretKeyThatIsAtLeast32CharactersLong!";

    [Fact]
    public void CreateToken_WithValidUser_ReturnsValidTokenWithCorrectClaims()
    {
        // Arrange
        var configuration = CreateMockConfiguration();
        var tokenService = new TokenService(configuration);
        var user = CreateValidUser();

        // Act
        var result = tokenService.CreateToken(user);

        // Assert - Token básico
        Assert.NotNull(result);
        Assert.NotNull(result.Token);
        Assert.True(result.Expiration > DateTime.UtcNow);
        Assert.Equal(user.Id, result.UserDto.Id);
        Assert.Equal(user.Email, result.UserDto.Email);

        // Assert - Claims del JWT
        var tokenHandler = new JwtSecurityTokenHandler();
        var jwtToken = tokenHandler.ReadJwtToken(result.Token);

        Assert.Equal("MazErpBack", jwtToken.Issuer);
        Assert.Equal("MazErpFront", jwtToken.Audiences?.FirstOrDefault());
        Assert.Equal(user.Id.ToString(), jwtToken.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value);
        Assert.Equal("user", jwtToken.Claims.First(c => c.Type == ClaimTypes.Role).Value);
    }

    [Fact]
    public void CreateToken_WithNullUser_ThrowsNullReferenceException()
    {
        var configuration = CreateMockConfiguration();
        var tokenService = new TokenService(configuration);

        Assert.Throws<NullReferenceException>(() => tokenService.CreateToken(null!));
    }

    [Fact]
    public void CreateToken_WhenJwtSecretIsMissing_ThrowsArgumentNullException()
    {
        var configuration = new Mock<IConfiguration>();
        configuration.Setup(x => x["JWT_SECRET"]).Returns((string)null!);

        var exception = Assert.Throws<ArgumentNullException>(() => new TokenService(configuration.Object));
        Assert.Contains("JWT_SECRET", exception.Message);
    }

    [Fact]
    public void CreateToken_WhenJwtSecretIsTooShort_ThrowsException()
    {
        var configuration = new Mock<IConfiguration>();
        configuration.Setup(x => x["JWT_SECRET"]).Returns("ShortSecret");

        var tokenService = new TokenService(configuration.Object);
        var user = CreateValidUser();

        Assert.Throws<ArgumentException>(() => tokenService.CreateToken(user));
    }

    [Fact]
    public void CreateToken_WithMinimalUserProperties_WorksCorrectly()
    {
        var configuration = CreateMockConfiguration();
        var tokenService = new TokenService(configuration);
        var user = new User
        {
            Id = 1,
            Email = "minimal@example.com",
            Name = null!,
            ProfilePhotoUrl = null
        };

        var result = tokenService.CreateToken(user);

        Assert.NotNull(result);
        Assert.NotNull(result.Token);
        Assert.Null(result.UserDto.Name);
        Assert.Null(result.UserDto.ProfilePhotoUrl);
    }

    [Fact]
    public void CreateToken_UsesEnvironmentVariableOverConfiguration()
    {
        var environmentSecret = "EnvironmentSecretKeyThatIsLongEnoughForJWT12345!";
        var originalEnvSecret = Environment.GetEnvironmentVariable("JWT_SECRET");

        try
        {
            Environment.SetEnvironmentVariable("JWT_SECRET", environmentSecret);

            var configuration = new Mock<IConfiguration>();
            configuration.Setup(x => x["JWT_SECRET"]).Returns("ShouldNotBeUsed");

            var tokenService = new TokenService(configuration.Object);
            var result = tokenService.CreateToken(CreateValidUser());

            Assert.NotNull(result.Token);
        }
        finally
        {
            Environment.SetEnvironmentVariable("JWT_SECRET", originalEnvSecret);
        }
    }

    [Fact]
    public void CreateToken_TokenCanBeValidated()
    {
        var configuration = CreateMockConfiguration();
        var tokenService = new TokenService(configuration);
        var result = tokenService.CreateToken(CreateValidUser());

        var tokenHandler = new JwtSecurityTokenHandler();
        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(ValidJwtSecret)),
            ValidateIssuer = true,
            ValidIssuer = "MazErpBack",
            ValidateAudience = true,
            ValidAudience = "MazErpFront",
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };

        var principal = tokenHandler.ValidateToken(result.Token, validationParameters, out var validatedToken);

        Assert.NotNull(principal);
        Assert.IsType<JwtSecurityToken>(validatedToken);
    }

    #region Métodos de ayuda

    private IConfiguration CreateMockConfiguration()
    {
        var configuration = new Mock<IConfiguration>();
        configuration.Setup(x => x["JWT_SECRET"]).Returns(ValidJwtSecret);
        configuration.Setup(x => x["Jwt:Issuer"]).Returns("MazErpBack");
        configuration.Setup(x => x["Jwt:Audience"]).Returns("MazErpFront");
        return configuration.Object;
    }

    private User CreateValidUser(int id = 1, string email = "test@example.com")
    {
        return new User
        {
            Id = id,
            Email = email,
            Name = "Test User",
            ProfilePhotoUrl = "https://example.com/photo.jpg",
            PasswordHash = "hashedpassword"
        };
    }

    #endregion
}