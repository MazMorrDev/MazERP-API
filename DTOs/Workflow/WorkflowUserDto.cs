using MazErpBack.Enums;

namespace MazErpBack.DTOs.Workflow;
public class WorkflowUserDto
{
    public int UserId { get; set; }
    public int WorkflowId { get; set; }
    public UserWorkflowRole Role { get; set; }
    public DateTimeOffset AssignedAt { get; set; }
}