using Microsoft.AspNetCore.Mvc;

namespace MazErpBack;

[ApiController]
[Route("api/[controller]")]
public class WarehouseController(WarehouseService warehouseService) : ControllerBase
{
    private readonly WarehouseService _warehouseService = warehouseService;

    [HttpPost]
    public async Task<IActionResult> CreateWarehouse([FromBody] CreateWarehouseDto warehouseDto)
    {
        try
        {
            return Ok(await _warehouseService.CreateWarehouse(warehouseDto));
        }
        catch (Exception)
        {
            throw;
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteWarehouse(int id)
    {
        try
        {
            return Ok(await _warehouseService.DeleteWarehouse(id));
        }
        catch (Exception)
        {
            throw;
        }
    }

    [HttpGet("by-workflow/{workflowId:int}")]
    public async Task<IActionResult> GetWarehousesByWorkflow(int workflowId)
    {
        try
        {
            return Ok(await _warehouseService.GetWarehousesByWorkflowAsync(workflowId));
        }
        catch (Exception)
        {
            throw;
        }
    }
}
