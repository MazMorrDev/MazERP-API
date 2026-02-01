using MazErpBack.DTOs.UserWorkflow;
using MazErpBack.Models;

namespace MazErpBack.Services;

public interface IUserWorkflowService
{
    public Task<UserWorkflow> AssignUserToWorkflow(AssignUserToWorkflowDto userWorkflowDto);
    public Task<UserWorkflow> ChangeUserRoleInWorkflow(ChangeUserRoleInWorkflowDto changeUserRoleInWorkflowDto);
    public Task<UserWorkflow> UpdateUserWorkflow(UpdateUserWorkflowDto userWorkflowDto);
    public Task<UserWorkflow> SoftDeleteUserWorkflow(DeleteUserWorkflowDto userWorkflowDto);

    // For very old registries that we don't want
    public Task DeleteUserWorkflow(DeleteUserWorkflowDto userWorkflowDto);
}
