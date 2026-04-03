using System.ComponentModel.DataAnnotations;

namespace MazErpBack.DTOs.Users;

public record class LoginDto
{
    [EmailAddress]
    public required string Email { get; init; }
    public required string Password { get; init; }
}