using Microsoft.AspNetCore.Mvc;

namespace MazErpBack;

[ApiController]
[Route("api/[controller]")]
public class WarehouseController(WarehouseService warehouseService) : ControllerBase
{
    private readonly WarehouseService _warehouseService = warehouseService;

    [HttpPost("create")]
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

    [HttpDelete("delete")]
    public async Task<IActionResult> DeleteWarehouse(DeleteWarehouseDto warehouseDto)
    {
        try
        {
            return Ok(await _warehouseService.DeleteWarehouse(warehouseDto));
        }
        catch(Exception)
        {
            throw;
        }
        
    }
}
