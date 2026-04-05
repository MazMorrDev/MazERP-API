using MazErpAPI.DTOs.Inventory;
using MazErpAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MazErpAPI.Controllers;

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
            // Validaciones
            if (pageNumber < 1 || pageSize < 1 || pageSize > 100)
                return BadRequest("Parámetros de paginación inválidos");

            var result = await _service.GetInventoriesByWarehouseAsync(warehouseId, pageNumber, pageSize);
            return Ok(result);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound($"Recurso no encontrado: {ex.Message}");
        }
        catch (UnauthorizedAccessException ex)
        {
            return Forbid($"Permiso denegado: {ex.Message}");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error interno del servidor: {ex.Message}");
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteInventory(int id, [FromHeader(Name = "companyId")] int companyId)
    {
        try
        {
            var deleted = await _service.SoftDeleteInventoryAsync(id);

            if (deleted)
                return NoContent(); // 204 - Eliminación exitosa
            else
                return NotFound($"Inventario con ID {id} no encontrado");
        }
        catch (InvalidOperationException ex)
        {
            // Ej: El inventario tiene movimientos asociados
            return Conflict(new
            {
                Error = "No se puede eliminar",
                Details = ex.Message
            });
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid(); // 403 - Sin permisos
        }
        catch (Exception ex)
        {
            // Log the exception here
            return StatusCode(500, $"Error interno del servidor {ex.Message}");
        }
    }

    [HttpPut("{inventoryId}")]
    public async Task<IActionResult> UpdateInventoryAndProduct(int inventoryId, [FromBody] UpdateInventoryProductDto dto, [FromHeader(Name = "companyId")] int companyId)
    {
        try
        {
            return Ok(await _service.UpdateInventoryAndProductAsync(inventoryId, dto));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound($"Recurso no encontrado: {ex.Message}");
        }
        catch (ArgumentException ex)
        {
            return BadRequest($"Datos inválidos: {ex.Message}");
        }
        catch (UnauthorizedAccessException ex)
        {
            return Forbid($"Permiso denegado: {ex.Message}");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error interno del servidor: {ex.Message}");
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateInventoryAndProduct([FromBody] CreateInventoryAndProductDto inventoryDto, [FromHeader(Name = "companyId")] int companyId)
    {
        try
        {
            return Ok(await _service.CreateInventoryAndProductAsync(inventoryDto));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound($"Recurso no encontrado: {ex.Message}");
        }
        catch (ArgumentException ex)
        {
            return BadRequest($"Datos inválidos: {ex.Message}");
        }
        catch (UnauthorizedAccessException ex)
        {
            return Forbid($"Permiso denegado: {ex.Message}");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error interno del servidor: {ex.Message}");
        }
    }

    [HttpPost("by-product")]
    public async Task<IActionResult> CreateInventoryByExistentProduct([FromBody] CreateInventoryByExistentProductDto inventoryDto, [FromHeader(Name = "companyId")] int companyId)
    {
        try
        {
            return Ok(await _service.CreateInventoryByExistentProductAsync(inventoryDto));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound($"Recurso no encontrado: {ex.Message}");
        }
        catch (ArgumentException ex)
        {
            return BadRequest($"Datos inválidos: {ex.Message}");
        }
        catch (UnauthorizedAccessException ex)
        {
            return Forbid($"Permiso denegado: {ex.Message}");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error interno del servidor: {ex.Message}");
        }
    }
}
