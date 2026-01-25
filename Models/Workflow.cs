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

    [Column("currency")]
    public Currency Currency { get; set; }

    // Auditoría
    [Column("created_at")]
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    [Column("updated_at")]
    public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;

    [Column("is_active")]
    public bool IsActive { get; set; } = true;

    // Navigation Properties
    public virtual ICollection<Warehouse> Warehouses { get; set; } = new HashSet<Warehouse>();
    public virtual ICollection<UserWorkflow> UserWorkflows { get; set; } = new HashSet<UserWorkflow>();
}
