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

    

    // Navigation Properties
    [ForeignKey("ProductId")]
    public required virtual Product Product { get; set; }

    [ForeignKey("SupplierId")]
    public required virtual Supplier Supplier { get; set; }
}
