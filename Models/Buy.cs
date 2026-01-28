using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MazErpBack.Enums;

namespace MazErpBack.Models;

[Table("Buy")]
public class Buy
{
    [Key, Column("movement_id")]
    public int MovementId { get; set; }

    [Required, Column("supplier_id")]
    public int SupplierId { get; set; }

    [Column("delivery_status")]
    public DeliveryStatus DeliveryStatus { get; set; }

    [Column("unitary_cost")]
    public decimal UnitaryCost { get; set; }

    [Column("received_quantity")]
    public int ReceivedQuantity { get; set; }

    // Navigation Property
    [ForeignKey("MovementId")]
    public required virtual Movement Movement { get; set; }

    [ForeignKey("SupplierId")]
    public required virtual User User { get; set; }
}
