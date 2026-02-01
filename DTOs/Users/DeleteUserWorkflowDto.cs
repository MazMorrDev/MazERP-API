namespace MazErpBack.DTOs.Users;

public record class DeleteUserWorkflowDto
{
    public int UserId { get; init; }
    public int WorkflowId { get; init; }
}
