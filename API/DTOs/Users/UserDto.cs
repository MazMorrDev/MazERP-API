using System.ComponentModel.DataAnnotations;

namespace MazErpBack.DTOs.Users;

public record class UserDto
{
    public int Id { get; init; }
    [EmailAddress]
    public required string Email { get; init; }
    public required string Name { get; init; }
    public string? ProfilePhotoUrl { get; init; }
}