namespace MazErpAPI.DTOs.Inventory;

public record class SellPointInventoryDto
{
    public int SellPointId { get; init; }
    public int InventoryId { get; init; }
    public decimal SellPrice { get; init; }
    public decimal Discount { get; init; } = 0;
    public required int Stock { get; init; }
    public int? WarningStock { get; init; }
    public int? AlertStock { get; init; }
    public DateTimeOffset? LastSaleDate { get; init; }
}
