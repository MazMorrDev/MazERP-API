using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MazErpBack.Enums;

namespace MazErpBack.Models;

[Table("Sell")]
public class Sell
{
    [Key, Column("movement_id")]
    public int MovementId { get; set; }

    [Column("discount_percentage", TypeName = "decimal(5,2)")]
    [Range(0, 100, ErrorMessage = "El porcentage de descuento no puede ser menor a 0 ni mayor a 100")]
    public decimal DiscountPercentage { get; set; }

    [Column("payment_status")]
    public PaymentStatus PaymentStatus { get; set; }

    [Column("sale_type")]
    public SaleType SaleType { get; set; }

    [Column("seller_notes")]
    public string? SellerNotes { get; set; }

    //Navigation Properties
    [ForeignKey("MovementId")]
    public required virtual Movement Movement {get; set;}
}
