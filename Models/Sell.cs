using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MazErpBack.Models;

[Table("Sell")]
public class Sell
{
    [Key, Column("id")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required, Column("invoice_number")]
    public required int InvoiceNumber { get; set; }

    [Column("discount_percentage", TypeName = "decimal(5,2)")]
    [Range(0, 100, ErrorMessage = "El porcentage de descuento no puede ser menor a 0 ni mayor a 100")]
    public decimal DiscountPercentage { get; set; }

    [Column("payment_status")]
    public PaymentStatus PaymentStatus { get; set; }

    [Column("sale_type")]
    public SaleType SaleType { get; set; }

    [Column("seller_notes")]
    public string? SellerNotes { get; set; }
}
