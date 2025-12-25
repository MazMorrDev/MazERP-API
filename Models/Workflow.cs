using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MazErpBack.Models;

[Table("Workflow")]
public class Workflow
{
    [Key, Column("id")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required, Column("name"), MaxLength(30)]
    public string Name { get; set; } = string.Empty;

    [Column("description"), MaxLength(255)]
    public string? Description { get; set; }

    [Column("workflow_photo_url"), MaxLength(500)]
    public string? WorkflowPhotoUrl { get; set; }

    [Column("created_at")]
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

    // Navigation Properties
    public virtual ICollection<Warehouse> Warehouses { get; set; } = new HashSet<Warehouse>();
    public virtual ICollection<ClientWorkflow> ClientWorkflows { get; set; } = new HashSet<ClientWorkflow>(); 
}
