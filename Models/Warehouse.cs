using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MazErpBack.Models;

namespace MazErpBack;

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

    // Navigation property
    [ForeignKey("WorkflowId")]
    public virtual Workflow Workflow { get; set; } = null!;
}
