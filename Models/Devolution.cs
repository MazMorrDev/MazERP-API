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

    [Column("reason"), MaxLength(99, ErrorMessage = " No puedes tener más de 99 letras")]
    public string? Reason { get; set; }

    [Column("refund_amount")]
    public int RefundAmount { get; set; }

    [Column("notes"), MaxLength(99, ErrorMessage = " No puedes tener más de 99 letras")]
    public string? Notes { get; set; }

    [Column("devolution_status")]
    public DevolutionStatus DevolutionStatus { get; set; }

    [Column("action_take")]
    public DevolutionActionTake ActionTake { get; set; }

    // Navigation Properties
    [ForeignKey("SellId")]
    public required virtual Sell Sell { get; set; }
}
