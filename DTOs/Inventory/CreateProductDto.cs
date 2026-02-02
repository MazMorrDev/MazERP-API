using System.ComponentModel.DataAnnotations;
using MazErpBack.Enums;

namespace MazErpBack.DTOs.Inventory;

public record class CreateProductDto
{
    [Required(ErrorMessage = "The product name is required")]
    [MaxLength(40)]
    public required string Name { get; init; }
    public string? Description { get; init; }
    [Url]
    public string? PhotoUrl { get; init; }
    public required ProductCategory ProductCategory { get; init; }
}
