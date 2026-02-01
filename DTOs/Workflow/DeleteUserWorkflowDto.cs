namespace MazErpBack.DTOs.Workflow;

public record class DeleteUserWorkflowDto
{
    public int UserId { get; init; }
    public int WorkflowId { get; init; }
}
