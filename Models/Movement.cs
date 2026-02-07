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
    public required int UserId { get; set; }

    [Required, Column("sell_point_id")]
    public required int SellPointId { get; set; }

    [Column("description"), MaxLength(225)]
    public string? Description { get; set; }

    [Required, Column("movement_type")]
    public MovementType MovementType { get; set; }

    [Required, Column("quantity")]
    public int Quantity { get; set; }

    [Column("currency")]
    public Currency Currency { get; set; }

    [Required, Column("movement_date")]
    public DateTimeOffset MovementDate { get; set; } 

    // Auditoría
    [Column("created_at")]
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

    [Column("updated_at")]
    public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;

    [Column("is_active")]
    public bool IsActive { get; set; } = true;

    // Navigation Properties
    [ForeignKey("UserId")]
    public required virtual User User { get; set; }

    [ForeignKey("SellPointId")]
    public required virtual SellPoint SellPoint { get; set; }
}
