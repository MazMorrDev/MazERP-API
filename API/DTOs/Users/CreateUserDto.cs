using System.ComponentModel.DataAnnotations;

namespace MazErpBack.DTOs.Users;

public record class CreateUserDto
{
    [EmailAddress]
    public required string Email { get; init; }

    public required string Name { get; init; }

    public required string Password { get; init; }

    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
    
    [Url]
    public string? ProfilePhotoUrl { get; init; }
}