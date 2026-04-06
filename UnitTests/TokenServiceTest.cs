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
    public void CreateToken_WithValidUser_ReturnsTokenDto()
    {
        // Arrange
        var configuration = CreateMockConfiguration();
        var tokenService = new TokenService(configuration);
        var user = CreateValidUser();

        // Act
        var result = tokenService.CreateToken(user);

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result.Token);
        Assert.NotEmpty(result.Token);
        Assert.True(result.Expiration > DateTime.UtcNow);
        Assert.NotNull(result.UserDto);
        Assert.Equal(user.Id, result.UserDto.Id);
        Assert.Equal(user.Email, result.UserDto.Email);
        Assert.Equal(user.Name, result.UserDto.Name);
        Assert.Equal(user.ProfilePhotoUrl, result.UserDto.ProfilePhotoUrl);
    }

    [Fact]
    public void CreateToken_WithValidUser_ReturnsValidJwtToken()
    {
        // Arrange
        var configuration = CreateMockConfiguration();
        var tokenService = new TokenService(configuration);
        var user = CreateValidUser();

        // Act
        var result = tokenService.CreateToken(user);
        
        // Assert - Validar que el token puede ser leído
        var tokenHandler = new JwtSecurityTokenHandler();
        Assert.True(tokenHandler.CanReadToken(result.Token));
        
        var jwtToken = tokenHandler.ReadJwtToken(result.Token);
        Assert.NotNull(jwtToken);
        Assert.Equal("MazErpBack", jwtToken.Issuer);
        Assert.Equal("MazErpFront", jwtToken.Audiences?.FirstOrDefault());
    }

    [Fact]
    public void CreateToken_WithValidUser_ContainsCorrectClaims()
    {
        // Arrange
        var configuration = CreateMockConfiguration();
        var tokenService = new TokenService(configuration);
        var user = CreateValidUser();

        // Act
        var result = tokenService.CreateToken(user);
        
        // Assert - Validar claims
        var tokenHandler = new JwtSecurityTokenHandler();
        var jwtToken = tokenHandler.ReadJwtToken(result.Token);
        
        var nameIdentifierClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
        var roleClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role);
        
        Assert.NotNull(nameIdentifierClaim);
        Assert.Equal(user.Id.ToString(), nameIdentifierClaim.Value);
        Assert.NotNull(roleClaim);
        Assert.Equal("user", roleClaim.Value);
    }

    [Fact]
    public void CreateToken_WithValidUser_SetsCorrectExpiration()
    {
        // Arrange
        var configuration = CreateMockConfiguration();
        var tokenService = new TokenService(configuration);
        var user = CreateValidUser();
        var beforeCreation = DateTime.UtcNow;

        // Act
        var result = tokenService.CreateToken(user);
        var afterCreation = DateTime.UtcNow;

        // Assert
        Assert.True(result.Expiration > beforeCreation);
        Assert.True(result.Expiration <= afterCreation.AddMinutes(35));
        Assert.True(result.Expiration - DateTime.UtcNow <= TimeSpan.FromMinutes(35));
    }

    [Fact]
    public void CreateToken_WithUserHavingMinimumProperties_WorksCorrectly()
    {
        // Arrange
        var configuration = CreateMockConfiguration();
        var tokenService = new TokenService(configuration);
        var user = new User
        {
            Id = 1,
            Email = "minimal@example.com",
            Name = null!, // Nombre nulo
            ProfilePhotoUrl = null
        };

        // Act
        var result = tokenService.CreateToken(user);

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result.Token);
        Assert.NotNull(result.UserDto);
        Assert.Null(result.UserDto.Name);
        Assert.Null(result.UserDto.ProfilePhotoUrl);
    }

    [Fact]
    public void CreateToken_WithUserHavingLongProperties_WorksCorrectly()
    {
        // Arrange
        var configuration = CreateMockConfiguration();
        var tokenService = new TokenService(configuration);
        var longName = new string('a', 500);
        var longEmail = new string('b', 200) + "@example.com";
        var user = new User
        {
            Id = int.MaxValue,
            Email = longEmail,
            Name = longName,
            ProfilePhotoUrl = "https://example.com/" + new string('c', 1000) + ".jpg"
        };

        // Act
        var result = tokenService.CreateToken(user);

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result.Token);
        Assert.Equal(longName, result.UserDto.Name);
        Assert.Equal(longEmail, result.UserDto.Email);
    }

    [Fact]
    public void CreateToken_WithNullUser_ThrowsNullReferenceException()
    {
        // Arrange
        var configuration = CreateMockConfiguration();
        var tokenService = new TokenService(configuration);

        // Act & Assert
        Assert.Throws<NullReferenceException>(() => tokenService.CreateToken(null!));
    }

    [Fact]
    public void CreateToken_WithUserHavingSpecialCharactersInEmail_WorksCorrectly()
    {
        // Arrange
        var configuration = CreateMockConfiguration();
        var tokenService = new TokenService(configuration);
        var user = new User
        {
            Id = 1,
            Email = "test+special@example.com",
            Name = "Special User",
            ProfilePhotoUrl = null
        };

        // Act
        var result = tokenService.CreateToken(user);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("test+special@example.com", result.UserDto.Email);
    }

    [Fact]
    public void CreateToken_WithUserHavingUnicodeCharactersInName_WorksCorrectly()
    {
        // Arrange
        var configuration = CreateMockConfiguration();
        var tokenService = new TokenService(configuration);
        var user = new User
        {
            Id = 1,
            Email = "unicode@example.com",
            Name = "José María ñandú 🚀",
            ProfilePhotoUrl = null
        };

        // Act
        var result = tokenService.CreateToken(user);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("José María ñandú 🚀", result.UserDto.Name);
        
        // Verificar que el token se puede leer correctamente
        var tokenHandler = new JwtSecurityTokenHandler();
        var jwtToken = tokenHandler.ReadJwtToken(result.Token);
        Assert.NotNull(jwtToken);
    }

    [Fact]
    public void CreateToken_WhenJwtSecretIsMissing_ThrowsArgumentNullException()
    {
        // Arrange
        var configuration = new Mock<IConfiguration>();
        configuration.Setup(x => x["JWT_SECRET"]).Returns((string)null!);
        configuration.Setup(x => x["Jwt:Issuer"]).Returns("TestIssuer");
        configuration.Setup(x => x["Jwt:Audience"]).Returns("TestAudience");

        // Act & Assert
        var exception = Assert.Throws<ArgumentNullException>(() => new TokenService(configuration.Object));
        Assert.Contains("JWT_SECRET", exception.Message);
    }

    [Fact]
    public void CreateToken_WhenJwtSecretIsTooShort_ThrowsException()
    {
        // Arrange
        var configuration = new Mock<IConfiguration>();
        configuration.Setup(x => x["JWT_SECRET"]).Returns("ShortSecret");
        configuration.Setup(x => x["Jwt:Issuer"]).Returns("TestIssuer");
        configuration.Setup(x => x["Jwt:Audience"]).Returns("TestAudience");

        var tokenService = new TokenService(configuration.Object);
        var user = CreateValidUser();

        // Act & Assert - Debe lanzar error por clave demasiado corta
        Assert.Throws<ArgumentException>(() => tokenService.CreateToken(user));
    }

    [Fact]
    public void CreateToken_UsesEnvironmentVariableOverConfiguration()
    {
        // Arrange
        var environmentSecret = "EnvironmentSecretKeyThatIsLongEnoughForJWT12345!";
        var configSecret = "ConfigSecretThatShouldNotBeUsed";
        
        // Guardar variable de entorno original
        var originalEnvSecret = Environment.GetEnvironmentVariable("JWT_SECRET");
        
        try
        {
            Environment.SetEnvironmentVariable("JWT_SECRET", environmentSecret);
            
            var configuration = new Mock<IConfiguration>();
            configuration.Setup(x => x["JWT_SECRET"]).Returns(configSecret);
            configuration.Setup(x => x["Jwt:Issuer"]).Returns("TestIssuer");
            configuration.Setup(x => x["Jwt:Audience"]).Returns("TestAudience");
            
            // Act
            var tokenService = new TokenService(configuration.Object);
            var user = CreateValidUser();
            var result = tokenService.CreateToken(user);
            
            // Assert - Si usó la variable de entorno, funcionará
            Assert.NotNull(result);
            Assert.NotNull(result.Token);
        }
        finally
        {
            // Restaurar variable de entorno original
            Environment.SetEnvironmentVariable("JWT_SECRET", originalEnvSecret);
        }
    }

    [Fact]
    public void CreateToken_UsesDefaultIssuerWhenNotInConfiguration()
    {
        // Arrange
        var configuration = new Mock<IConfiguration>();
        configuration.Setup(x => x["JWT_SECRET"]).Returns(ValidJwtSecret);
        configuration.Setup(x => x["Jwt:Issuer"]).Returns((string)null!);
        configuration.Setup(x => x["Jwt:Audience"]).Returns((string)null!);
        
        var tokenService = new TokenService(configuration.Object);
        var user = CreateValidUser();
        
        // Act
        var result = tokenService.CreateToken(user);
        
        // Assert
        var tokenHandler = new JwtSecurityTokenHandler();
        var jwtToken = tokenHandler.ReadJwtToken(result.Token);
        Assert.Equal("MazErpBack", jwtToken.Issuer);
        Assert.Equal("MazErpFront", jwtToken.Audiences?.FirstOrDefault());
    }

    [Fact]
    public void CreateToken_WithDifferentUsers_GeneratesDifferentTokens()
    {
        // Arrange
        var configuration = CreateMockConfiguration();
        var tokenService = new TokenService(configuration);
        var user1 = CreateValidUser(id: 1, email: "user1@example.com");
        var user2 = CreateValidUser(id: 2, email: "user2@example.com");
        
        // Act
        var result1 = tokenService.CreateToken(user1);
        var result2 = tokenService.CreateToken(user2);
        
        // Assert
        Assert.NotEqual(result1.Token, result2.Token);
    }

    [Fact]
    public void CreateToken_TokenCanBeValidated()
    {
        // Arrange
        var configuration = CreateMockConfiguration();
        var tokenService = new TokenService(configuration);
        var user = CreateValidUser();
        
        // Act
        var result = tokenService.CreateToken(user);
        
        // Assert - Validar el token
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
        Assert.NotNull(validatedToken);
        Assert.IsType<JwtSecurityToken>(validatedToken);
    }

    [Fact]
    public void CreateToken_WhenTokenGenerationFails_ThrowsApplicationException()
    {
        // Arrange - Simular configuración inválida que cause error
        var configuration = new Mock<IConfiguration>();
        configuration.Setup(x => x["JWT_SECRET"]).Returns(""); // Secret vacío causa error
        configuration.Setup(x => x["Jwt:Issuer"]).Returns("TestIssuer");
        configuration.Setup(x => x["Jwt:Audience"]).Returns("TestAudience");
        
        var tokenService = new TokenService(configuration.Object);
        var user = CreateValidUser();
        
        // Act & Assert
        var exception = Assert.Throws<ApplicationException>(() => tokenService.CreateToken(user));
        Assert.Contains($"Error creating token for user {user.Email}", exception.Message);
        Assert.NotNull(exception.InnerException);
    }

    [Fact]
    public void CreateToken_WithUserHavingZeroId_WorksCorrectly()
    {
        // Arrange
        var configuration = CreateMockConfiguration();
        var tokenService = new TokenService(configuration);
        var user = new User
        {
            Id = 0,
            Email = "zero@example.com",
            Name = "Zero ID User",
            ProfilePhotoUrl = null
        };
        
        // Act
        var result = tokenService.CreateToken(user);
        
        // Assert
        Assert.NotNull(result);
        var tokenHandler = new JwtSecurityTokenHandler();
        var jwtToken = tokenHandler.ReadJwtToken(result.Token);
        var idClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
        Assert.Equal("0", idClaim?.Value);
    }

    [Fact]
    public void CreateToken_WithUserHavingNegativeId_WorksCorrectly()
    {
        // Arrange
        var configuration = CreateMockConfiguration();
        var tokenService = new TokenService(configuration);
        var user = new User
        {
            Id = -1,
            Email = "negative@example.com",
            Name = "Negative ID User",
            ProfilePhotoUrl = null
        };
        
        // Act
        var result = tokenService.CreateToken(user);
        
        // Assert
        Assert.NotNull(result);
        var tokenHandler = new JwtSecurityTokenHandler();
        var jwtToken = tokenHandler.ReadJwtToken(result.Token);
        var idClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
        Assert.Equal("-1", idClaim?.Value);
    }

    [Fact]
    public void CreateToken_ExpirationIsExactly35MinutesFromNow()
    {
        // Arrange
        var configuration = CreateMockConfiguration();
        var tokenService = new TokenService(configuration);
        var user = CreateValidUser();
        var now = DateTime.UtcNow;
        
        // Act
        var result = tokenService.CreateToken(user);
        
        // Assert - Permitir una pequeña diferencia por tiempo de ejecución
        var difference = result.Expiration - now.AddMinutes(35);
        Assert.True(Math.Abs(difference.TotalSeconds) < 5, 
            $"Expiration should be 35 minutes from now. Actual difference: {difference.TotalSeconds} seconds");
    }

    [Fact]
    public void CreateToken_ReturnsSameUserDtoStructure()
    {
        // Arrange
        var configuration = CreateMockConfiguration();
        var tokenService = new TokenService(configuration);
        var user = CreateValidUser();
        
        // Act
        var result = tokenService.CreateToken(user);
        
        // Assert - Verificar que UserDto no es la misma referencia pero tiene los mismos datos
        Assert.NotSame(user, result.UserDto);
        Assert.Equal(user.Id, result.UserDto.Id);
        Assert.Equal(user.Email, result.UserDto.Email);
        Assert.Equal(user.Name, result.UserDto.Name);
        Assert.Equal(user.ProfilePhotoUrl, result.UserDto.ProfilePhotoUrl);
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
            PasswordHash = "hashedpassword" // No se usa para el token
        };
    }

    #endregion
}