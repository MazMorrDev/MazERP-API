using MazErpBack.Dtos;
using MazErpBack.Dtos.Login;
using MazErpBack.Services.User;
using Microsoft.AspNetCore.Mvc;

namespace MazErpBack.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly UserService _userService;
    public UserController(UserService userService)
    {
        _userService = userService;
    }
    [HttpPost("register")]
    public async Task<IActionResult> RegisterUser([FromBody] CreateUserDto createUserDto)
    {
        try
        {
            var createUser = await _userService.RegisterUserAsync(createUserDto);
            return Ok(new { data = createUser } );
        }
        catch (Exception)
        {
            return StatusCode(500, new { error = "An error occurred while processing your request." });
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
    {
        try
        {
            var token = await _userService.LoginUserAsync(loginDto);
            if (token == null)
            {
                return Unauthorized(new { error = "Invalid email or password." });
            }
            return Ok(new { token = token.Token, expiration = token.Expiration });
        }
        catch (System.Exception)
        {
            throw;
        }
    }
}