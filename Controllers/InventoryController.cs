using MazErpBack.DTOs.Inventory;
using MazErpBack.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MazErpBack.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class InventoryController(IInventoryService service) : ControllerBase
{
    private readonly IInventoryService _service = service;

    [HttpGet("by-warehouse{warehouseId}")]
    public async Task<IActionResult> GetInventoriesByWarehouse(
        int warehouseId, [FromHeader(Name = "companyId")] int companyId,
        [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10
    )
    {
        try
        {
            return Ok(await _service.GetInventoriesByWarehouseAsync(warehouseId, pageNumber, pageSize));
        }
        catch (Exception)
        {
            throw;
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteInventory(int id, [FromHeader(Name = "companyId")] int companyId)
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
    public async Task<IActionResult> UpdateInventoryAndProduct(int inventoryId, [FromBody] UpdateInventoryProductDto dto, [FromHeader(Name = "companyId")] int companyId)
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
    public async Task<IActionResult> CreateInventoryAndProduct([FromBody] CreateInventoryAndProductDto inventoryDto, [FromHeader(Name = "companyId")] int companyId)
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
    public async Task<IActionResult> CreateInventoryByExistentProduct([FromBody] CreateInventoryByExistentProductDto inventoryDto, [FromHeader(Name = "companyId")] int companyId)
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
