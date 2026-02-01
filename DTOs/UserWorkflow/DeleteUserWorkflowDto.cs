namespace MazErpBack.DTOs.UserWorkflow;

public record class DeleteUserWorkflowDto
{
    int UserId { get; init; }
    int WorkflowId { get; init; }
}
