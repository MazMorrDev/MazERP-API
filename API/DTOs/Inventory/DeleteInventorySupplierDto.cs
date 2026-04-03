using System.ComponentModel.DataAnnotations;

namespace MazErpBack.DTOs.Inventory;

public record class DeleteInventorySupplierDto
{
    [Required]
    public int InventoryId { get; init; }
    [Required]
    public int SupplierId { get; init; }
}
