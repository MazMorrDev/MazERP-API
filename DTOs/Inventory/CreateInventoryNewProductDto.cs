using System.ComponentModel.DataAnnotations;

namespace MazErpBack.DTOs.Inventory;

public record class CreateInventoryNewProductDto
{
    [Required]
    public int WarehouseId { get; init; }
    [Required]
    public int Stock { get; init; }
    public decimal BaseDiscount { get; init; }
    public decimal? BasePrice { get; init; }
    public decimal AverageCost { get; init; }
    public int? AlertStock { get; init; }
    public int? WarningStock { get; init; }
    public DateTimeOffset? LastSaleDate { get; init; }
}
