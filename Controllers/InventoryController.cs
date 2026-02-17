using MazErpBack.DTOs.Inventory;
using MazErpBack.Services.Interfaces;
using MazErpBack.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MazErpBack.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class InventoryController(IInventoryService service, HeaderHelper header) : ControllerBase
{
    private readonly IInventoryService _service = service;
    private readonly HeaderHelper _header = header;

    [HttpGet("by-warehouse{warehouseId}")]
    public async Task<IActionResult> GetInventoriesByWarehouse(int warehouseId)
    {
        try
        {
            var companyId = _header.GetCompanyIdFromHeader();
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

    [HttpPut("{inventoryId}")]
    public async Task<IActionResult> UpdateInventoryAndProduct(int inventoryId, [FromBody] UpdateInventoryProductDto dto)
    {
        try
        {
            return Ok(await _service.UpdateInventoryAndProductAsync(inventoryId, dto));
        }
        catch (Exception)
        {
            throw;
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateInventoryAndProduct([FromBody] CreateInventoryAndProductDto inventoryDto)
    {
        try
        {
            return Ok(await _service.CreateInventoryAndProductAsync(inventoryDto));
        }
        catch (Exception)
        {
            throw;
        }
    }

    [HttpPost("by-product")]
    public async Task<IActionResult> CreateInventoryByExistentProduct([FromBody] CreateInventoryByExistentProductDto inventoryDto)
    {
        try
        {
            return Ok(await _service.CreateInventoryByExistentProductAsync(inventoryDto));
        }
        catch (Exception)
        {
            throw;
        }
    }
}
