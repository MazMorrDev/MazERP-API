using MazErpAPI.DTOs.Movements;
using MazErpAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MazErpAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class DevolutionController(IDevolutionService service, ILogger<DevolutionController> logger) : ControllerBase
{
    public IDevolutionService _service = service;
    public ILogger<DevolutionController> _logger = logger;

    [HttpGet]
    public async Task<IActionResult> GetDevolutions(
        [FromQuery] int pageNumber = 1, 
        [FromQuery] int pageSize = 10,
        [FromHeader(Name = "companyId")] int companyId = 0)
    {
        try
        {
            // Validaciones
            if (pageNumber < 1 || pageSize < 1 || pageSize > 100)
                return BadRequest("Parámetros de paginación inválidos");

            // Nota: El servicio no tiene un método para obtener todas las devoluciones por compañía directamente
            // Esto sería una mejora futura en el servicio
            // Por ahora, retornaremos BadRequest indicando que se necesita especificar un filtro
            return BadRequest("Especifique un filtro: por inventario (inventoryId) o por punto de venta (sellPointId)");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error en GetDevolutions");
            return StatusCode(500, $"Error interno del servidor: {ex.Message}");
        }
    }

    [HttpGet("by-inventory/{inventoryId}")]
    public async Task<IActionResult> GetDevolutionsByInventory(
        int inventoryId, 
        [FromQuery] int pageNumber = 1, 
        [FromQuery] int pageSize = 10,
        [FromHeader(Name = "companyId")] int companyId = 0)
    {
        try
        {
            // Validaciones
            if (pageNumber < 1 || pageSize < 1 || pageSize > 100)
                return BadRequest("Parámetros de paginación inválidos");

            var result = await _service.GetDevolutionsByInventoryAsync(inventoryId, pageNumber, pageSize);
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
            _logger.LogError(ex, "Error en GetDevolutionsByInventory");
            return StatusCode(500, $"Error interno del servidor: {ex.Message}");
        }
    }

    [HttpGet("by-sellpoint/{sellPointId}")]
    public async Task<IActionResult> GetDevolutionsBySellPoint(
        int sellPointId, 
        [FromQuery] int pageNumber = 1, 
        [FromQuery] int pageSize = 10,
        [FromHeader(Name = "companyId")] int companyId = 0)
    {
        try
        {
            // Validaciones
            if (pageNumber < 1 || pageSize < 1 || pageSize > 100)
                return BadRequest("Parámetros de paginación inválidos");

            var result = await _service.GetDevolutionsBySellPointAsync(sellPointId, pageNumber, pageSize);
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
            _logger.LogError(ex, "Error en GetDevolutionsBySellPoint");
            return StatusCode(500, $"Error interno del servidor: {ex.Message}");
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetDevolutionById(int id)
    {
        try
        {
            var result = await _service.GetDevolutionByIdAsync(id);
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
            _logger.LogError(ex, "Error en GetDevolutionById");
            return StatusCode(500, $"Error interno del servidor: {ex.Message}");
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateDevolution([FromBody] CreateDevolutionDto createDevolutionDto)
    {
        try
        {
            var result = await _service.CreateDevolutionAsync(createDevolutionDto);
            return CreatedAtAction(nameof(GetDevolutionById), new { id = result.DevolutionId }, result);
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
            _logger.LogError(ex, "Error en CreateDevolution");
            return StatusCode(500, $"Error interno del servidor: {ex.Message}");
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateDevolution(int id, [FromBody] CreateDevolutionDto updateDevolutionDto)
    {
        try
        {
            // Nota: El servicio actual no tiene un método UpdateDevolutionAsync
            // Esto sería una mejora futura en el servicio
            // Por ahora, retornaremos NotImplemented
            var result = await _service.UpdateDevolutionAsync(id, updateDevolutionDto);
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
            _logger.LogError(ex, "Error en UpdateDevolution");
            return StatusCode(500, $"Error interno del servidor: {ex.Message}");
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteDevolution(int id)
    {
        try
        {
            await _service.DeleteDevolutionAsync(id);
            return NoContent();
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
            _logger.LogError(ex, "Error en DeleteDevolution");
            return StatusCode(500, $"Error interno del servidor: {ex.Message}");
        }
    }
}
