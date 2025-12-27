using Microsoft.AspNetCore.Mvc;

namespace MazErpBack;

[ApiController]
public class InventoryController(InventoryService inventoryService) : ControllerBase
{
    private readonly InventoryService _inventoryService = inventoryService;

    [HttpGet("by-warehouse{warehouseId:int}")]
    public async Task<IActionResult> GetInventoriesByWarehouse(int warehouseId)
    {
        try
        {
            return Ok(await _inventoryService.GetInventoriesByWarehouseAsync(warehouseId));
        }
        catch (Exception)
        {
            throw;
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteInventory(int id)
    {
        try
        {
            return Ok(await _inventoryService.DeleteInventoryAsync(id));
        }
        catch (Exception)
        {
            throw;
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateInventory([FromBody] CreateInventoryDto inventoryDto)
    {
        try
        {
            return Ok(await _inventoryService.CreateInventoryAsync(inventoryDto));
        }
        catch (Exception)
        {
            throw;
        }
    }
}
