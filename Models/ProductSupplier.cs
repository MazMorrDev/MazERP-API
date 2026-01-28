
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MazErpBack.Models;

[Table("Product_Supplier")]
public class ProductSupplier
{
    [Required, Column("product_id")]
    public required int ProductId { get; set; }

    [Required, Column("supplier_id")]
    public required int SupplierId { get; set; }

    [Column("cost_price")]
    public decimal CostPrice { get; set; }

    [Column("currency")]
    public Currency Currency { get; set; }

    [Column("load_time_days")]
    public int LoadTimeDays { get; set; }

    [Column("min_order_quantity")]
    public int MinOrderQuantity { get; set; }

    [Column("rating")]
    [Range(0, 5, ErrorMessage = "el rating no puede ser menor que 0 ni mayor que 5")]
    public int Rating { get; set; }

    [Column("is_preferred")]
    public bool IsPreferred { get; set; }

    [Column("last_purchase_date")]
    public DateTimeOffset LastPurchaseDate { get; set; } = DateTimeOffset.UtcNow;

    [Column("notes")]
    public string? Notes { get; set; }

    // Navigation Properties
    [ForeignKey("ProductId")]
    public required virtual Product Product { get; set; }

    [ForeignKey("SupplierId")]
    public required virtual Supplier Supplier { get; set; }
}
