using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MazErpBack.Enums;

namespace MazErpBack.Models;

[Table("User_Workflow")]
public class UserWorkflow
{
    [Key, Column("user_id", Order = 0)]
    public int UserId { get; set; }


    [Key, Column("workflow_id", Order = 1)]
    public int WorkflowId { get; set; }

    [Column("role")]
    public UserWorkflowRole Role { get; set; }

    [Column("assigned_at")]
    public DateTimeOffset AssignedAt { get; set; } = DateTimeOffset.UtcNow;

    [Column("updated_at")]
    public DateTimeOffset UpdatedAt {get; set;} = DateTimeOffset.UtcNow;

    [Column("is_active")]
    public bool IsActive {get; set;} = true;

    // Navigation properties
    [ForeignKey("UserId")]
    public virtual User User { get; set; } = null!;

    [ForeignKey("WorkflowId")]
    public virtual Workflow Workflow { get; set; } = null!;
}