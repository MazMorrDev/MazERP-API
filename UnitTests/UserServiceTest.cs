using MazErpAPI.Context;
using MazErpAPI.Controllers;
using MazErpAPI.Services.Implementation;
using MazErpAPI.Services.Interfaces;
using MazErpBack.Services.Implementation;
using Microsoft.Extensions.Logging;

namespace UnitTests;

public class UserServiceTest
{

    private readonly UserController _controller;
    private readonly ILogger<IUserService> _logger;
    private readonly IUserService _userService;
    private readonly AppDbContext _context;
    private readonly ITokenService _tokenService;

    public UserServiceTest(){
        _tokenService = new TokenService();
        _userService = new UserService(_context, );
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
