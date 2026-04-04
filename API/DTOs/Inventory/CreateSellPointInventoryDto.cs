using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MazErpAPI.DTOs.Inventory;

public record class CreateSellPointInventoryDto
{
    [Required]
    public int SellPointId { get; init; }
    [Required]
    public int InventoryId { get; init; }

    [Column(TypeName = "decimal(12,2)")]
    public decimal SellPrice { get; init; }

    [Column("discount", TypeName = "decimal(5,2)")]
    [Range(0, 100, ErrorMessage = "El descuento debe estar entre 0 y 100%")]
    public decimal Discount { get; init; } = 0;

    [Required]
    public required int Stock { get; init; }

    public int? WarningStock { get; init; }

    public int? AlertStock { get; init; }
}
