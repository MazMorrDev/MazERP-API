using MazErpBack.DTOs.Workflow;
using MazErpBack.Enums;
using MazErpBack.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MazErpBack.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class WfController(IWorkflowService wfService) : ControllerBase
{
    private readonly IWorkflowService _wfService = wfService;

    [HttpGet("workflows")]
    [Authorize(Roles = "Client")]
    public async Task<IActionResult> GetWorkflows()
    {
        var workflows = await _wfService.GetWorkflowsAsync();
        return Ok(new { data = workflows });
    }

    [HttpPut("assign/{userId}/{workflowId}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> AssignWorkflowToUser(int userId, int workflowId, [FromQuery] UserWorkflowRole role = UserWorkflowRole.Admin)
    {
        try
        {
            return Ok(await _wfService.AssignWorkflowToUser(userId, workflowId, role));
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return StatusCode(500, "An error occurred while assigning the workflow to the user. Check logs for details or try again later.");
        }
    }

    [HttpPost("create")]
    [Authorize(Roles = "Admin")] // necesito el role para crear un wf ademas del admin
    public async Task<IActionResult> CreateWorkflow([FromBody] CreateWorkflowDto workflow)
    {
        try
        {
            return Ok(await _wfService.CreateWorkflow(workflow));
        }
        catch (Exception)
        {

            throw;
        }
    }

}
