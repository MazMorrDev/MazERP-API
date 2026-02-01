using MazErpBack.DTOs.UserWorkflow;
using MazErpBack.Models;

namespace MazErpBack.Services;

public interface IUserWorkflowService
{
    // This method have to ensure that if an user is already at the wf then update the values
    public Task<UserWorkflow> AssignUserToWorkflow(AssignUserRoleToWorkflowDto userWorkflowDto);
    public Task<UserWorkflow> SoftDeleteUserWorkflow(DeleteUserWorkflowDto userWorkflowDto);

    // For very old registries that we don't want
    public Task DeleteUserWorkflow(DeleteUserWorkflowDto userWorkflowDto);
}
