using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MazErpBack;

[Table("Movement")]
public class Movement
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [Column("client_id")]
    public int ClientId { get; set; }

    [Required]
    [Column("warehouse_id")]
    public int WarehouseId { get; set; }

    [Required]
    [Column("product_id")]
    public int ProductId { get; set; }

    [MaxLength(225)]
    [Column("description")]
    public string? Description { get; set; }

    [Required]
    [Column("movement_type")]
    public MovementType MovementType { get; set; }

    [Required]
    [Column("quantity")]
    public int Quantity { get; set; }

    [Required]
    [Column("unitary_cost")]
    public double UnitaryCost { get; set; }

    [Required]
    [Column("movement_date")]
    public DateTime MovementDate { get; set; }

    // Navigation Properties
    [ForeignKey("ClientId")]
    public virtual Client Client { get; set; } = null!;

    [ForeignKey("WarehouseId")]
    public virtual Warehouse Warehouse { get; set; } = null!;

    [ForeignKey("ProductId")]
    public virtual Product Product { get; set; } = null!;

}
