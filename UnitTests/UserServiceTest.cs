using MazErpAPI.Context;
using MazErpAPI.Controllers;
using MazErpAPI.DTOs.Users;
using MazErpAPI.Models;
using MazErpAPI.Services.Implementation;
using MazErpAPI.Services.Interfaces;
using MazErpAPI.Utils.Mappers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
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
    public async Task LoginUser_WithInvalidPassword_ReturnsArgumentException()
    {
        // Arrange
        var user = new User
        {
            Id = 1,
            Email = "test@example.com",
            Name = "testuser",
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

        // Execution
        var result = await _userService.LoginUserAsync(loginDto);

        // Assert
        Assert.Null(result);
    }
}
