using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MazErpBack.Models;

[Table("Client_Workflow")]
public class ClientWorkflow
{
    [Key, Column("client_id", Order = 0)]
    public int ClientId { get; set; }


    [Key, Column("workflow_id", Order = 1)]
    public int WorkflowId { get; set; }

    [Column("role")]
    public ClientWorkflowRole Role { get; set; }

    [Column("assigned_at")]
    public DateTimeOffset AssignedAt { get; set; } = DateTimeOffset.UtcNow;


    // Navigation properties
    [ForeignKey("ClientId")]
    public virtual Client Client { get; set; } = null!;

    [ForeignKey("WorkflowId")]
    public virtual Workflow Workflow { get; set; } = null!;
}