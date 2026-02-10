using System.ComponentModel.DataAnnotations;
using MazErpBack.Enums;

namespace MazErpBack.DTOs.Inventory;

public record class ProductDto
{
    public required int ProductId { get; init; }
    public required string Name { get; init; }
    public string? Description { get; init; }
    [Url]
    public string? PhotoUrl { get; init; }
    public required ProductCategory ProductCategory { get; init; }
}
