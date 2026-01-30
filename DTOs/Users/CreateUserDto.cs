using System.ComponentModel.DataAnnotations;

namespace MazErpBack.Dtos.Users;

public class CreateUserDto
{
    [EmailAddress]
    public required string Email { get; set; }

    public required string Name { get; set; }

    public required string Password { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public string? ProfilePhotoUrl { get; set; }
}