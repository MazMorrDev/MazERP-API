using System.ComponentModel.DataAnnotations;
using MazErpBack.Enums;

namespace MazErpBack.DTOs.Inventory;

public record class CreateInventoryAndProductDto
{
    [Required(ErrorMessage = "The product name is required")]
    [MaxLength(40)]
    public required string ProductName { get; init; }
    public string? ProductDescription { get; init; }
    [Url]
    public string? PhotoUrl { get; init; }
    public required ProductCategory ProductCategory { get; init; }
    [Required]
    public int WarehouseId { get; init; }
    [Required]
    public int Stock { get; init; }
    public decimal BaseDiscount { get; init; }
    public decimal? BasePrice { get; init; }
    public decimal AverageCost { get; init; }
    public int? AlertStock { get; init; }
    public int? WarningStock { get; init; }
}
