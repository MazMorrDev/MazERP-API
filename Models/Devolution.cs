using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MazErpBack.Models;

public class Devolution
{
    [Key, Column("id")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Column("sell_id")]
    public required int SellId { get; set; }

    [Column("reason")]
    public string? Reason { get; set; }

    [Column("refund_amount")]
    public int RefundAmount { get; set; }

    [Column("notes")]
    public string? Notes { get; set; }

    [Column("devolution_status")]
    public DevolutionStatus DevolutionStatus { get; set; }

    [Column("action_take")]
    public DevolutionActionTake ActionTake { get; set; }

    // Navigation Properties
    [ForeignKey("SellId")]
    public required virtual Sell Sell { get; set; }
}
