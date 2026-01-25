using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MazErpBack.Enums;

namespace MazErpBack.Models;

[Table("Movement")]
public class Movement
{
    [Key, Column("id")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required, Column("user_id")]
    public int UserId { get; set; }

    [Required, Column("warehouse_id")]
    public int WarehouseId { get; set; }

    [Required, Column("product_id")]
    public int ProductId { get; set; }

    [Column("description"), MaxLength(225)]
    public string? Description { get; set; }

    [Required, Column("movement_type")]
    public MovementType MovementType { get; set; }

    [Required, Column("quantity")]
    public int Quantity { get; set; }

    [Column("unitary_cost")]
    public decimal? UnitaryCost { get; set; }

    [Required, Column("movement_date")]
    public DateTimeOffset MovementDate { get; set; }

    // Navigation Properties
    [ForeignKey("UserId")]
    public virtual User User { get; set; } = null!;

    [ForeignKey("WarehouseId")]
    public virtual Warehouse Warehouse { get; set; } = null!;

    [ForeignKey("ProductId")]
    public virtual Product Product { get; set; } = null!;

}
