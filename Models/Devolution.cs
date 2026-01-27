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

    

    // Navigation Properties
    [ForeignKey("SellId")]
    public required virtual Sell Sell { get; set; }
}
