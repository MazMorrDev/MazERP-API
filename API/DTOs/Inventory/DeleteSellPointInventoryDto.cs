using System.ComponentModel.DataAnnotations;

namespace MazErpAPI.DTOs.Inventory;

public record class DeleteSellPointInventoryDto
{
    [Required]
    public int SellPointId { get; init; }
    [Required]
    public int InventoryId { get; init; }
}
