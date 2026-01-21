using MazErpBack.Context;
using MazErpBack.Dtos.Workflow;
using MazErpBack.Enums;
using MazErpBack.Models;
using MazErpBack.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MazErpBack.Services;

public class WfService(AppDbContext context, ILogger<WfService> logger) : IWorkflowService
{
    private readonly AppDbContext _context = context;
    private readonly ILogger<WfService> _logger = logger;

    public async Task<WorkflowUserDto> AssignWorkflowToUser(int userId, int workflowId, UserWorkflowRole role = UserWorkflowRole.Admin)
    {
        try
        {
            var existingUserWf = await _context.UserWorkflows.FirstOrDefaultAsync(cw => cw.UserId == userId && cw.WorkflowId == workflowId);
            if (existingUserWf != null)
            {
                throw new BadHttpRequestException($"Workflow {workflowId} is already assigned to user {userId}.");
            }
            var userWfAdd = new UserWorkflow
            {
                UserId = userId,
                WorkflowId = workflowId,
                Role = role,
                AssignedAt = DateTimeOffset.UtcNow
            };
            // check if workflow is already associated to user
            _context.UserWorkflows.Add(userWfAdd);
            await _context.SaveChangesAsync();
            return new WorkflowUserDto
            {
                UserId = userWfAdd.UserId,
                WorkflowId = userWfAdd.WorkflowId,
                Role = userWfAdd.Role,
                AssignedAt = userWfAdd.AssignedAt
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error assigning workflow with id:{workflowId} to client with id:{userId}");
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
            // FIX: cualquier cliente no debería tener acceso a todos los workflows
            var result = await _context.Workflows.ToListAsync();
            return result;
        }
        catch (Exception)
        {
            throw;
        }
    }
}
