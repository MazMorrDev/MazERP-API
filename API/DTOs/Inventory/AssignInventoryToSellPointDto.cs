using System.ComponentModel.DataAnnotations;

namespace MazErpAPI.DTOs.Inventory;

public record class AssignInventoryToSellPointDto
{
    public required int SellPointId { get; init; }

    public required int InventoryId { get; init; }

    public decimal SellPrice { get; init; }

    [Range(0, 100, ErrorMessage = "El descuento debe estar entre 0 y 100%")]
    public decimal Discount { get; init; }

    public required int Stock { get; init; }

    public int? WarningStock { get; init; }

    public int? AlertStock { get; init; }
}
