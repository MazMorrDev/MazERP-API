using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MazErpBack;

[Table("ClientWorkflows")]
public class ClientWorkflow
{
    // Clave primaria compuesta - Parte 1
    [Column("client_id")]
    public int ClientId { get; set; }

    // Clave primaria compuesta - Parte 2
    [Column("workflow_id")]
    public int WorkflowId { get; set; }

    [MaxLength(500)]
    [Column("notes")]
    public ClientWorkflowRole Role { get; set; }

    [Column("assigned_at")]
    public DateTimeOffset AssignedAt { get; set; } = DateTimeOffset.UtcNow;


    // Navigation properties
    [ForeignKey("ClientId")]
    public virtual Client Client { get; set; } = null!;

    [ForeignKey("WorkflowId")]
    public virtual Workflow Workflow { get; set; } = null!;
}