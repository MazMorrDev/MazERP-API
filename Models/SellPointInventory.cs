using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MazErpBack.Models;

[Table("Sell_Point_Inventory")]
public class SellPointInventory
{
    [Key, Column("sell_point_id")]
    public int SellPointId { get; set; }

    [Key, Column("inventory_id")]
    public int InventoryId { get; set; }

    [Column("sell_price", TypeName = "decimal(12,2)")]
    public decimal SellPrice { get; set; }

    [Column("discount", TypeName = "decimal(5,2)")]
    [Range(0, 100, ErrorMessage = "El descuento debe estar entre 0 y 100%")]
    public decimal Discount { get; set; } = 0;

    [Required, Column("stock")]
    public required int Stock { get; set; }

    [Column("warning_stock")]
    public int WarningStock { get; set; }

    [Column("alert_stock")]
    public int AlertStock { get; set; }

    [Column("last_sale_date")]
    public DateTimeOffset? LastSaleDate { get; set; }

    // Navigation properties
    [ForeignKey("SellPointId")]
    public virtual required SellPoint SellPoint { get; set; }
    [ForeignKey("InventoryId")]
    public virtual required Inventory Inventory { get; set; }
}
