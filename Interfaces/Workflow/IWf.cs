using MazErpBack.Dtos.Workflow;
using MazErpBack.Models;

namespace MazErpBack.Interfaces;

public interface IWf
{
    public Task<List<Workflow>> GetWfAsync();

    public Task<Workflow> CreateWorkflow(CreateWorkflowDto workflowDto);

    public Task<WorkflowClientDto> AssingnWorkflowToClient(int clientId, int workflowId, ClientWorkflowRole role = ClientWorkflowRole.Admin);
}
