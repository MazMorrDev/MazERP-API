using MazErpBack.Enums;

namespace MazErpBack.DTOs.Workflow;

public record class AssignUserRoleToWorkflowDto
{
    public int UserId { get; init; }
    public int WorkflowId { get; init; }
    public UserWorkflowRole UserWorkflowRole {get; init;} = UserWorkflowRole.Member;
}
