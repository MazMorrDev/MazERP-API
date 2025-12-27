using System.ComponentModel.DataAnnotations;

namespace MazErpBack;

public record class CreateInventoryDto
{
    [Required]
    public int WarehouseId { get; set; }

    [Required]
    public int ProductId { get; set; }

    [Required]
    public int Stock { get; set; }

    public int? AlertStock { get; set; }

    public int? WarningStock { get; set; }

}
