using MazErpBack.DTOs.Users;
using MazErpBack.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MazErpBack.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController(IUserService userService) : ControllerBase
{
    private readonly IUserService _userService = userService;

    [HttpPost("register")]
    public async Task<IActionResult> RegisterUser([FromBody] CreateUserDto createUserDto)
    {
        try
        {
            var createUser = await _userService.RegisterUserAsync(createUserDto);
            return Ok(createUser);
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
            return Ok(token);
        }
        catch (Exception)
        {
            throw;
        }
    }

    // [HttpPost("license-update")] // Hay que aclarar este sistema además del sistema de pago
}
