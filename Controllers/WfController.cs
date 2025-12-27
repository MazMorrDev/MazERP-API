using MazErpBack.Dtos.Workflow;
using MazErpBack.Services.WorkflowService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace MazErpBack.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class WfController(WfService wfService) : ControllerBase
{
    private readonly WfService _wfService = wfService;

    [HttpGet("workflows")]
    [Authorize(Roles = "Client")]
    public async Task<IActionResult> GetWorkflows()
    {
        var workflows = await _wfService.GetWfAsync();
        return Ok(new { data = workflows });
    }

    [HttpPut("assign/{clientId}/{workflowId}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> AssignWorkflowToClient(int clientId, int workflowId, [FromQuery] ClientWorkflowRole role = ClientWorkflowRole.Admin)
    {
        try
        {
            return Ok(await _wfService.AssingnWorkflowToClient(clientId, workflowId, role));
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return StatusCode(500, "An error occurred while assigning the workflow to the client. Check logs for details or try again later.");
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
