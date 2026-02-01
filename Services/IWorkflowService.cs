using MazErpBack.DTOs.Workflow;
using MazErpBack.Enums;
using MazErpBack.Models;

namespace MazErpBack.Services;

public interface IWorkflowService
{
    // Para el panel de administración
    public Task<List<Workflow>> GetWorkflowsAsync();
    public Task DeleteWorkflowAsync(int workflowId);

    // Para el usuario común
    public Task<Workflow> CreateWorkflowAsync(CreateWorkflowDto workflowDto);
    public Task<Workflow> SoftDeleteWorkflow(int workflowId);
    public Task<WorkflowUserDto> AssignWorkflowToUserAsync(int userId, int workflowId, UserWorkflowRole role = UserWorkflowRole.Admin);
}
