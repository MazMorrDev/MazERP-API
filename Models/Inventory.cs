using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MazErpBack.Models;

[Table("Inventory")]
public class Inventory
{
    [Key, Column("id")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required, Column("warehouse_id")]
    public int WarehouseId { get; set; }

    [Required, Column("product_id")]
    public int ProductId { get; set; }

    [Required, Column("stock")]
    public int Stock { get; set; }

    [Column("actual_discount", TypeName = "decimal(5,2)")]
    [Range(0, 100, ErrorMessage = "El descuento debe estar entre 0 y 100%")]
    public decimal? ActualDiscount { get; set; }

    [Column("average_cost")]
    public decimal AverageCost { get; set; }

    // // Lógica para alertas
    // public bool IsBelowAlertStock => AlertStock.HasValue && Stock < AlertStock.Value;

    // public bool IsBelowWarningStock => WarningStock.HasValue && Stock < WarningStock.Value;

    [Column("alert_stock")]
    public int? AlertStock { get; set; }

    [Column("warning_stock")]
    public int? WarningStock { get; set; }

    // Auditoría
    [Column("last_sale_date")]
    public DateTimeOffset LastSaleDate { get; set; }

    [Column("updated_at")]
    public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;

    [Column("is_active")]
    public bool IsActive { get; set; } = true;

    // Navigation Properties
    [ForeignKey("WarehouseId")]
    public virtual Warehouse Warehouse { get; set; } = null!;

    [ForeignKey("ProductId")]
    public virtual Product Product { get; set; } = null!;

    public virtual ICollection<Movement> Movements { get; set; } = new HashSet<Movement>();
}
