namespace MazErpBack.DTOs.Users;

public record UserDto
{
    public int Id { get; init; }
    public required string Email { get; init; }
    public required string Name { get; init; }
    public string? ProfilePhotoUrl { get; init; }

    //TODO: agregar propiedades de la relacion con Workflow y Movement
}