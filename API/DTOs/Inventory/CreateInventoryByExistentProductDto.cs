using System.ComponentModel.DataAnnotations;
using MazErpBack.Enums;

namespace MazErpBack.DTOs.Inventory;

public record class CreateInventoryByExistentProductDto
{
    [Required]
    public required int ProductId { get; init; }
    [Required]
    public required int WarehouseId { get; init; }
    [Required]
    public int Stock { get; init; }
    public decimal BaseDiscount { get; init; }
    public decimal? BasePrice { get; init; }
    public decimal AverageCost { get; init; }
    public int? AlertStock { get; init; }
    public int? WarningStock { get; init; }
}
