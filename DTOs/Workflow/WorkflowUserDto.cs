using MazErpBack.Enums;

namespace MazErpBack.DTOs.Workflow;
public record WorkflowUserDto
{
    public int UserId { get; init; }
    public int WorkflowId { get; init; }
    public UserWorkflowRole Role { get; init; }
    public DateTimeOffset AssignedAt { get; init; }
}