using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MazErpBack.Enums;

namespace MazErpBack.Models;

public class Devolution
{
    [Key, Column("id")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Column("sell_id")]
    public required int SellId { get; set; }

    [Column("reason"), MaxLength(120, ErrorMessage = "Reason No puede tener más de 120 letras")]
    public string? Reason { get; set; }

    [Column("refund_amount")]
    public int RefundAmount { get; set; }

    [Column("notes"), MaxLength(120, ErrorMessage = "Notes No puede tener más de 120 letras")]
    public string? Notes { get; set; }

    [Column("devolution_status")]
    public DevolutionStatus DevolutionStatus { get; set; }

    [Column("action_take")]
    public DevolutionActionTake DevolutionActionTake { get; set; }

    [Column("date")]
    public DateTimeOffset DevolutionDate { get; set; }

    [Column("updated_at")]
    public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.Now;

    [Column("is_active")]
    public bool IsActive { get; set; } = true;

    // Navigation Properties
    [ForeignKey("SellId")]
    public required virtual Sell Sell { get; set; }
}
