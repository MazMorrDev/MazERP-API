using MazErpBack.Dtos.Workflow;
using MazErpBack.Interfaces;
using MazErpBack.Models;
using Microsoft.EntityFrameworkCore;

namespace MazErpBack.Services.WorkflowService;

public class WfService(AppDbContext context, ILogger<WfService> logger) : IWf
{
    private readonly AppDbContext _context = context;
    private readonly ILogger<WfService> _logger = logger;

    public async Task<WorkflowClientDto> AssingnWorkflowToClient(int clientId, int workflowId, ClientWorkflowRole role = ClientWorkflowRole.Admin)
    {
        try
        {
            var existingClientWf = await _context.ClientWorkflows
                .FirstOrDefaultAsync(cw => cw.ClientId == clientId && cw.WorkflowId == workflowId);
            if (existingClientWf != null)
            {
                throw new BadHttpRequestException($"Workflow {workflowId} is already assigned to client {clientId}.");
            }
            var clientWfAdd = new ClientWorkflow
            {
                ClientId = clientId,
                WorkflowId = workflowId,
                Role = role,
                AssignedAt = DateTimeOffset.UtcNow
            };
            // check if workflow is already associated to client
            _context.ClientWorkflows.Add(clientWfAdd);
            await _context.SaveChangesAsync();
            return new WorkflowClientDto
            {
                ClientId = clientWfAdd.ClientId,
                WorkflowId = clientWfAdd.WorkflowId,
                Role = clientWfAdd.Role,
                AssignedAt = clientWfAdd.AssignedAt
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error assigning workflow with id:{workflowId} to client with id:{clientId}");
            throw;
        }
    }

    public async Task<Workflow> CreateWorkflow(CreateWorkflowDto workflowDto)
    {
        try
        {
            var workflow = new Workflow()
            {
                Name = workflowDto.Name,
                Description = workflowDto.Description,
                WorkflowPhotoUrl = workflowDto.WorkflowPhotoUrl,
                CreatedAt = workflowDto.CreatedAt
            };

            _context.Workflows.Add(workflow);

            await _context.SaveChangesAsync();

            return workflow;
        }
        catch (Exception)
        {

            throw;
        }
    }

    public async Task<List<Workflow>> GetWfAsync()
    {
        try
        {
            var result = await _context.Workflows.ToListAsync();
            return result;
        }
        catch (Exception)
        {
            throw;
        }
    }
}
