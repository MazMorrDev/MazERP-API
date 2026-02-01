using MazErpBack.DTOs.Workflow;
using MazErpBack.Enums;
using MazErpBack.Models;

namespace MazErpBack.Services;

public interface IWorkflowService
{
    public Task<List<Workflow>> GetWorkflowsAsync();

    public Task<Workflow> CreateWorkflowAsync(CreateWorkflowDto workflowDto);

    public Task<WorkflowUserDto> AssignWorkflowToUserAsync(int userId, int workflowId, UserWorkflowRole role = UserWorkflowRole.Admin);
}
