using System.ComponentModel.DataAnnotations;

namespace MazErpBack.DTOs.Inventory;

public record class CreateSellPointDto
{
    [Required]
    public required string Name { get; set; }
    public string? Description { get; set; }
    public string? Location { get; set; }
}
