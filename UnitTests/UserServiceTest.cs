using MazErpBack.Controllers;
using MazErpBack.Services.Implementation;
using MazErpBack.Services.Interfaces;

namespace UnitTests;

public class UserServiceTest
{
    private readonly UserController _controller ;
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
