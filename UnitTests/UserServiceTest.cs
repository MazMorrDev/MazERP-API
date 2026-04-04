using MazErpAPI.Controllers;
using MazErpAPI.Services.Implementation;
using MazErpAPI.Services.Interfaces;

namespace UnitTests;

public class UserServiceTest
{
    private readonly 
    private readonly UserController _controller;
    private readonly IUserService _service;
    public UserServiceTest()
    {
        _service = new UserService();
        _controller = new UserController(_service);
    }


    [Fact]
    public void LoginUser_WithInvalidPassword_ReturnsNull()
    {
        // Arrange
        var options = CreateNewContextOptions();
    }
}
