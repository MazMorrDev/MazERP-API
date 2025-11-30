using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MazErpBack;

[Table("Warehouse")]
public class Warehouse
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public int Id { get; set; }

    // Foreign Key para Workflow
    [Required]
    [Column("workflow_id")]
    public int WorkflowId { get; set; }

    [Required]
    [MaxLength(30)]
    [Column("name")]
    public string Name { get; set; } = string.Empty;

    [MaxLength(255)]
    [Column("description")]
    public string? Description { get; set; }

    // Navigation property
    [ForeignKey("WorkflowId")]
    public virtual Workflow Workflow { get; set; } = null!;
}
