using MazErpAPI.DTOs.Users;
using MazErpAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MazErpAPI.Controllers;

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
        catch (ArgumentException ex)
        {
            return BadRequest($"Datos inválidos: {ex.Message}");
        }
        catch (InvalidOperationException ex)
        {
            return Conflict($"Conflicto: {ex.Message}");
        }
        catch (Exception ex)
        {
            // Log the exception here (assuming there's a logger available)
            return StatusCode(500, new { error = $"An error occurred while processing your request: {ex.Message}" });
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
        catch (ArgumentException ex)
        {
            return BadRequest($"Datos inválidos: {ex.Message}");
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound($"Recurso no encontrado: {ex.Message}");
        }
        catch (Exception ex)
        {
            // Log the exception here
            return StatusCode(500, new { error = $"An error occurred while processing your request: {ex.Message}." });
        }
    }

    // [HttpPost("license-update")] // Hay que aclarar este sistema además del sistema de pago
}
