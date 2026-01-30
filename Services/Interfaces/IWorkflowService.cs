using MazErpBack.Dtos.Workflow;
using MazErpBack.Enums;
using MazErpBack.Models;

namespace MazErpBack.Services.Interfaces;

public interface IWorkflowService
{
    public Task<List<Workflow>> GetWorkflowsAsync();

    public Task<Workflow> CreateWorkflow(CreateWorkflowDto workflowDto);

    public Task<WorkflowUserDto> AssignWorkflowToUser(int userId, int workflowId, UserWorkflowRole role = UserWorkflowRole.Admin);
}
