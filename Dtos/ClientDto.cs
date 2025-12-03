namespace MazErpBack.Dtos;

public class ClientDto
{
    public int Id { get; set; }
    public required string Email { get; set; }
    public required string Name { get; set; }
    public string? ProfilePhotoUrl { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }

    //todo: agregar propiedades de la relacion con Workflow y Movement
}