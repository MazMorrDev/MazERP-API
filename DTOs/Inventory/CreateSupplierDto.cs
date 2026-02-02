using System.ComponentModel.DataAnnotations;

namespace MazErpBack.DTOs.Inventory;

public record class CreateSupplierDto
{
    public required string Name { get; init; }
    [EmailAddress]
    public string? Email { get; init; }
    [Phone]
    public string? PhoneNumber { get; init; }
    public string? Location { get; init; }
    public int? Rating { get; init; }
}
