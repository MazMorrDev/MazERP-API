using MazErpBack.DTOs.UserWorkflow;
using MazErpBack.Models;

namespace MazErpBack.Services;

public interface IUserWorkflowService
{
    public Task<UserWorkflow> AssignUserToWorkflow(AssignUserRoleToWorkflowDto userWorkflowDto);
    public Task<UserWorkflow> ChangeUserRoleInWorkflow(AssignUserRoleToWorkflowDto userWorkflowDto);
    public Task<UserWorkflow> UpdateUserWorkflow(UpdateUserWorkflowDto userWorkflowDto);
    public Task<UserWorkflow> SoftDeleteUserWorkflow(DeleteUserWorkflowDto userWorkflowDto);

    // For very old registries that we don't want
    public Task DeleteUserWorkflow(DeleteUserWorkflowDto userWorkflowDto);
}
