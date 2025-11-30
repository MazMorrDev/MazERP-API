using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MazErpBack;

[Table("Inventory")]
public class Inventory
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [Column("warehouse_id")]
    public int WarehouseId { get; set; }

    [Required]
    [Column("product_id")]
    public int ProductId { get; set; }

    // Navigation Properties
    [ForeignKey("WarehouseId")]
    public virtual Warehouse Warehouse { get; set; } = null!;

    [ForeignKey("ProductId")]
    public virtual Product Product { get; set; } = null!;
}
