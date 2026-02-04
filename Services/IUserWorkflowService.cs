using MazErpBack.DTOs.Workflow;

namespace MazErpBack.Services;

public interface IUserWorkflowService
{
    // This method has to ensure that if an user is already at the wf then update the values
    public Task<UserWorkflowDto> AssignUserWithRoleToWorkflowAsync(UserWorkflowDto userWorkflowDto);
    public Task<bool> SoftDeleteUserWorkflowAsync(DeleteUserWorkflowDto userWorkflowDto);

    // For very old registries that we don't want
    public Task<bool> DeleteUserWorkflowAsync(DeleteUserWorkflowDto userWorkflowDto);
}
