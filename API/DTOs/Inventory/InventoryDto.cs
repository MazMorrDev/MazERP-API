using System.ComponentModel.DataAnnotations;
using MazErpBack.Enums;

namespace MazErpBack.DTOs.Inventory;

public record class InventoryDto
{
    public required int ProductId { get; init; }
    public required string ProductName { get; init; }
    public string? ProductDescription { get; init; }
    [Url]
    public string? PhotoUrl { get; init; }
    public required ProductCategory ProductCategory { get; init; }
    public required int InventoryId { get; init; }
    public required int WarehouseId { get; init; }
    public required int Stock { get; init; }
    public decimal BaseDiscount { get; init; }
    public decimal? BasePrice { get; init; }
    public decimal AverageCost { get; init; }
    public int? AlertStock { get; init; }
    public int? WarningStock { get; init; }
}
