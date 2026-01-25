using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MazErpBack.Models;

public class Expense
{
    [Key, Column("id")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required, Column("user_id")]
    public int UserId { get; set; }

    [Required, Column("inventory_id")]
    public int InventoryId { get; set; }

    [Column("description")]
    public string? Description { get; set; }

    [Required, Column("category")]
    public required ExpenseCategory Category { get; set; }

    [Column("amount")]
    public int Amount { get; set; }

    [Column("payment_method")]
    public PaymentMethod PaymentMethod { get; set; }

    // Auditoría
    [Column("date_paid")]
    public DateTimeOffset DatePaid { get; set; }

    [Column("created_at")]
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

    [Column("updated_at")]
    public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;

    [Column("is_active")]
    public bool IsActive { get; set; } = true;

    // Navigation Properties
    [ForeignKey("UserId")]
    public virtual User User { get; set; } = null!;

    [ForeignKey("InventoryId")]
    public virtual Inventory Inventory { get; set; } = null!;
}
