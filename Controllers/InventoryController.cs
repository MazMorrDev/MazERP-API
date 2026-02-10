using MazErpBack.DTOs.Inventory;
using MazErpBack.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MazErpBack.Controllers;

[ApiController]
[Route("api/[controller]")]
public class InventoryController(IInventoryService service) : ControllerBase
{
    private readonly IInventoryService _service = service;

    [HttpGet("by-warehouse{warehouseId:int}")]
    public async Task<IActionResult> GetInventoriesByWarehouse(int warehouseId)
    {
        try
        {
            return Ok(await _service.GetInventoriesByWarehouseAsync(warehouseId));
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
            return Ok(await _service.SoftDeleteInventoryAsync(id));
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
            return Ok(await _service.CreateInventoryAsync(inventoryDto));
        }
        catch (Exception)
        {
            throw;
        }
    }
}
