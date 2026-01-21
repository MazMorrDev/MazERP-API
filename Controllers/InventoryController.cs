using MazErpBack.Dtos.Inventory;
using MazErpBack.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MazErpBack.Controllers;

[ApiController]
[Route("api/[controller]")]
public class InventoryController(IInventoryService inventoryService) : ControllerBase
{
    private readonly IInventoryService _inventoryService = inventoryService;

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
