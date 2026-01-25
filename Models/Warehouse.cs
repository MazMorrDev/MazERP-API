using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MazErpBack.Models;

[Table("Warehouse")]
public class Warehouse
{
    [Key, Column("id")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    // Foreign Key para Workflow
    [Required, Column("workflow_id")]
    public int WorkflowId { get; set; }

    [Required, Column("name"), MaxLength(30)]
    public string Name { get; set; } = string.Empty;

    [Column("description"), MaxLength(255)]
    public string? Description { get; set; }

    [Column("updated_at")]
    public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;

    [Column("is_active")]
    public bool IsActive { get; set; } = true;

    // Navigation property
    [ForeignKey("WorkflowId")]
    public virtual Workflow Workflow { get; set; } = null!;
}
