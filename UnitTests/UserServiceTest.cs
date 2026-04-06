using MazErpAPI.Context;
using MazErpAPI.DTOs.Users;
using MazErpAPI.Models;
using MazErpAPI.Services.Implementation;
using MazErpAPI.Services.Interfaces;
using MazErpAPI.Utils.Mappers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace UnitTests;

public class UserServiceTest
{
    private readonly Mock<ILogger<UserService>> _mockLogger;
    private readonly Mock<ITokenService> _mockTokenService;
    private readonly UserMapper _mapper;
    private readonly AppDbContext _context;
    private readonly UserService _userService;

    public UserServiceTest()
    {
        _mockLogger = new Mock<ILogger<UserService>>();
        _mockTokenService = new Mock<ITokenService>();
        _mapper = new UserMapper();

        // Configurar base de datos en memoria
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDb_" + Guid.NewGuid())
            .Options;

        _context = new AppDbContext(options);
        _userService = new UserService(_context, _mockTokenService.Object, _mockLogger.Object, _mapper);
    }

    [Fact]
    public async Task LoginUser_WithCorrectCredentials_ReturnsTokenDto()
    {
        // Arrange
        var user = new User
        {
            Id = 1,
            Email = "test@example.com",
            Name = "testuser",
            IsActive = true,
            LicenseStartDate = DateTimeOffset.UtcNow,
            LicenseEndDate = DateTimeOffset.UtcNow.AddMonths(1)
        };

        // IMPORTANTE: Usar el mismo PasswordHasher que usa el servicio
        var hasher = new PasswordHasher<User>();
        user.PasswordHash = hasher.HashPassword(user, "password123");

        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();

        // Verificar que el usuario se guardó correctamente
        var savedUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == "test@example.com");
        Assert.NotNull(savedUser); // Debug: verificar que existe
        Assert.True(savedUser.IsActive); // Debug: verificar que está activo
        Assert.NotNull(savedUser.PasswordHash); // Debug: verificar que tiene hash

        var loginDto = new LoginDto
        {
            Email = "test@example.com",
            Password = "password123"
        };

        var userDto = _mapper.MapToDto(user);
        // Setup mock token service para que retorne algo
        var expectedToken = new TokenDto { Token = "test-token-123", Expiration = DateTime.Now.AddHours(1), UserDto = userDto };
        _mockTokenService.Setup(x => x.CreateToken(It.IsAny<User>())).Returns(expectedToken);

        // Act
        var result = await _userService.LoginUserAsync(loginDto);

        // Assert
        Assert.NotNull(result); // Cambiar a Assert.NotNull primero
        Assert.IsType<TokenDto>(result);
    }


    [Fact]
    public async Task LoginUser_WithInvalidPassword_ReturnsNull()
    {
        // Arrange
        var user = new User
        {
            Id = 1,
            Email = "test@example.com",
            Name = "testuser",
            IsActive = true,
            LicenseStartDate = DateTimeOffset.UtcNow,
            LicenseEndDate = DateTimeOffset.UtcNow.AddMonths(1)
        };
        var hasher = new PasswordHasher<User>();
        user.PasswordHash = hasher.HashPassword(user, "password123");

        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();

        var loginDto = new LoginDto
        {
            Email = "pepito@gmail.com",
            Password = "password123"
        };

        // Act
        var result = await _userService.LoginUserAsync(loginDto);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task LoginUser_WithUnexistentEmail_ReturnsNull()
    {
        var loginDto = new LoginDto
        {
            Email = "pepito@gmail.com",
            Password = "password123"
        };

        // Execution
        var result = await _userService.LoginUserAsync(loginDto);

        // Assert
        Assert.Null(result);
    }


}
