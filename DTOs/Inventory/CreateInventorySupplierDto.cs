using System.ComponentModel.DataAnnotations;
using MazErpBack.Enums;

namespace MazErpBack.DTOs.Inventory;

public record class CreateInventorySupplierDto
{
    [Required]
    public int InventoryId { get; init; }
    [Required]
    public int SupplierId { get; init; }
    public decimal? CostPrice { get; init; }
    public Currency Currency { get; init; }
    public int? LoadTimeDays { get; init; }
    public int? MinOrderQuantity { get; init; }
    public bool IsPreferred { get; init; }
    public string? Notes { get; init; }
}
