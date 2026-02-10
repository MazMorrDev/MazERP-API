using MazErpBack.DTOs.Inventory;
using MazErpBack.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MazErpBack.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WarehouseController(IWarehouseService warehouseService) : ControllerBase
{
    private readonly IWarehouseService _warehouseService = warehouseService;

    [HttpPost]
    public async Task<IActionResult> CreateWarehouse([FromBody] CreateWarehouseDto warehouseDto)
    {
        try
        {
            return Ok(await _warehouseService.CreateWarehouseAsync(warehouseDto));
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
            return Ok(await _warehouseService.SoftDeleteWarehouseAsync(id));
        }
        catch (Exception)
        {
            throw;
        }
    }

    [HttpGet("by-company/{companyId:int}")]
    public async Task<IActionResult> GetWarehousesByCompany(int companyId)
    {
        try
        {
            return Ok(await _warehouseService.GetWarehousesByCompanyAsync(companyId));
        }
        catch (Exception)
        {
            throw;
        }
    }
}
