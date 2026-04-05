using MazErpAPI.DTOs.Inventory;
using MazErpAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MazErpAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class SupplierController(ISupplierService service, ILogger<SupplierController> logger) : ControllerBase
{
    public ISupplierService _service = service;
    public ILogger<SupplierController> _logger = logger;

    [HttpGet]
    public async Task<IActionResult> GetSuppliersByWarehouse(
        [FromHeader(Name = "companyId")] int companyId,
        [FromQuery] int warehouseId,
        [FromQuery] int pageNumber = 1, 
        [FromQuery] int pageSize = 10
    )
    {
        try
        {
            // Validaciones
            if (pageNumber < 1 || pageSize < 1 || pageSize > 100)
                return BadRequest("Parámetros de paginación inválidos");

            var result = await _service.GetSuppliersByWarehouseAsync(warehouseId, pageNumber, pageSize);
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
            // Logear el error aquí
            _logger.LogError(ex, "Error en GetSuppliersByWarehouse");
            return StatusCode(500, $"Error interno del servidor: {ex.Message}");
        }
    }

    [HttpGet("{supplierId}")]
    public async Task<IActionResult> GetSupplierById(
        int supplierId, 
        [FromHeader(Name = "companyId")] int companyId
    )
    {
        try
        {
            var result = await _service.GetSupplierByIdAsync(supplierId);
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
            // Logear el error aquí
            _logger.LogError(ex, "Error en GetSupplierById");
            return StatusCode(500, $"Error interno del servidor: {ex.Message}");
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateSupplier(
        [FromBody] CreateSupplierDto createSupplierDto, 
        [FromHeader(Name = "companyId")] int companyId
    )
    {
        try
        {
            return Ok(await _service.CreateSupplierAsync(createSupplierDto));
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
            _logger.LogError(ex, "Error en CreateSupplier");
            return StatusCode(500, $"Error interno del servidor: {ex.Message}");
        }
    }

    [HttpPut("{supplierId}")]
    public async Task<IActionResult> UpdateSupplier(
        [FromBody] CreateSupplierDto supplierDto, 
        int supplierId, 
        [FromHeader(Name = "companyId")] int companyId
    )
    {
        try
        {
            return Ok(await _service.UpdateSupplierAsync(supplierId, supplierDto));
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
            _logger.LogError(ex, "Error en UpdateSupplier");
            return StatusCode(500, $"Error interno del servidor: {ex.Message}");
        }
    }

    [HttpDelete("{supplierId}")]
    public async Task<IActionResult> DeleteSupplier(
        int supplierId, 
        [FromHeader(Name = "companyId")] int companyId
    )
    {
        try
        {
            var deleted = await _service.SoftDeleteSupplierAsync(supplierId);

            if (deleted)
                return NoContent(); // 204 - Eliminación exitosa
            else
                return NotFound($"Proveedor con ID {supplierId} no encontrado");
        }
        catch (InvalidOperationException ex)
        {
            // Ej: El proveedor tiene movimientos asociados
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
            _logger.LogError(ex, "Error en DeleteSupplier");
            return StatusCode(500, $"Error interno del servidor {ex.Message}");
        }
    }
}