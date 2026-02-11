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
        catch (KeyNotFoundException ex)
        {
            return NotFound($"Recurso no encontrado: {ex.Message}");
        }
        catch (ArgumentException ex)
        {
            return BadRequest($"Datos inválidos: {ex.Message}");
        }
        catch (InvalidOperationException ex)
        {
            return Conflict($"Conflicto: {ex.Message}");
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized($"No autorizado: {ex.Message}");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error interno del servidor: {ex.Message}");
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteWarehouse(int id)
    {
        try
        {
            var deleted = await _warehouseService.SoftDeleteWarehouseAsync(id);

            if (deleted)
                return NoContent(); // 204 - Eliminación exitosa
            else
                return NotFound($"Almacén con ID {id} no encontrado");
        }
        catch (InvalidOperationException ex)
        {
            // Ej: El almacén tiene productos asociados
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

    [HttpGet("by-company/{companyId:int}")]
    public async Task<IActionResult> GetWarehousesByCompany(int companyId)
    {
        try
        {
            var warehouses = await _warehouseService.GetWarehousesByCompanyAsync(companyId);

            if (warehouses == null || warehouses.Count == 0)
                return NotFound($"No se encontraron almacenes para la empresa {companyId}");

            return Ok(warehouses);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound($"Empresa con ID {companyId} no encontrada: {ex.Message}");
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

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetWarehouseById(int id)
    {
        try
        {
            var warehouse = await _warehouseService.GetWarehouseByIdAsync(id);

            if (warehouse == null) return NotFound($"Almacén con ID {id} no encontrado");

            return Ok(warehouse);
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
