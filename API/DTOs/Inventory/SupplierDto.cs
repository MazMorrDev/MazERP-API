using System.ComponentModel.DataAnnotations;

namespace MazErpAPI.DTOs.Inventory;

public record class SupplierDto
{
    public required int SupplierId { get; init; }
    public required string Name { get; init; }
    public string? ContactPerson { get; set; }
    [EmailAddress]
    public string? Email { get; init; }
    [Phone]
    public string? PhoneNumber { get; init; }
    public string? Location { get; init; }
    public int? Rating { get; init; }
}
