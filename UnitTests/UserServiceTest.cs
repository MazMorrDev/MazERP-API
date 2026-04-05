using MazErpAPI.Context;
using MazErpAPI.Controllers;
using MazErpAPI.Services.Implementation;
using MazErpAPI.Services.Interfaces;
using MazErpAPI.Utils.Mappers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;

namespace UnitTests;

public class UserServiceTest
{
    private readonly Mock<IConfiguration> _mockConfig;
    private readonly Mock<ILogger<UserService>> _mockLogger;
    private readonly Mock<ITokenService> _mockTokenService;
    private readonly UserMapper _mapper;
    private readonly AppDbContext _context;
    private readonly UserService _userService;
    private readonly UserController _controller;

    public UserServiceTest()
    {
        _mockConfig = new Mock<IConfiguration>();
        _mockLogger = new Mock<ILogger<UserService>>();
        _mockTokenService = new Mock<ITokenService>();
        _mapper = new UserMapper();

        // Configurar base de datos en memoria
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDb_" + Guid.NewGuid())
            .Options;

        _context = new AppDbContext(options);
        _userService = new UserService(_context, _mockTokenService.Object, _mockLogger.Object, _mapper);
        _controller = new UserController(_userService);
    }


    [Fact]
    public void LoginUser_WithInvalidPassword_ReturnsNull()
    {
        // Arrange


        // Execution


        // Assert
        Assert
    }
}
