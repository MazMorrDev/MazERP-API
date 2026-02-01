using MazErpBack.Enums;

namespace MazErpBack.DTOs.UserWorkflow;

public record class AssignUserRoleToWorkflowDto
{
    public int UserId { get; init; }
    public int WorkflowId { get; init; }
    public UserWorkflowRole UserWorkflowRole {get; init;} = UserWorkflowRole.Member;
}
