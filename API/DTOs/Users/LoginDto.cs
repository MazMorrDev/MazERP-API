using System.ComponentModel.DataAnnotations;

namespace MazErpAPI.DTOs.Users;

public record class LoginDto
{
    [EmailAddress]
    public required string Email { get; init; }
    public required string Password { get; init; }
}