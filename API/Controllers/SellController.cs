using MazErpBack.DTOs.Movements;
using MazErpBack.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MazErpBack.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class SellController(ISellService service, ILogger<SellController> logger) : ControllerBase
{
    public ISellService _service = service;
    public ILogger<SellController> _logger = logger;

    [HttpGet("by-inventory/{sellPointId}")]
    public async Task<IActionResult> GetSellsBySellPoint(
        int sellPointId, [FromHeader(Name = "companyId")] int companyId,
        [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10
    )
    {
        try
        {
            // Validaciones
            if (pageNumber < 1 || pageSize < 1 || pageSize > 100)
                return BadRequest("Parámetros de paginación inválidos");

            var result = await _service.GetSellsBySellPointAsync(sellPointId, pageNumber, pageSize);
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
            _logger.LogError(ex, "Error en GetSellsBySellPoint");
            return StatusCode(500, $"Error interno del servidor: {ex.Message}");
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateSell([FromBody] CreateSellDto createSellDto, [FromHeader(Name = "companyId")] int companyId)
    {
        try
        {
            return Ok(await _service.CreateSellAsync(createSellDto));
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
            _logger.LogError(ex, "Error en CreateSell");
            return StatusCode(500, $"Error interno del servidor: {ex.Message}");
        }
    }

    [HttpDelete("{sellId}")]
    public async Task<IActionResult> DeleteSell(int sellId, [FromHeader(Name = "companyId")] int companyId)
    {
        try
        {
            var deleted = await _service.SoftDeleteSellAsync(sellId);

            if (deleted)
                return NoContent(); // 204 - Eliminación exitosa
            else
                return NotFound($"Venta con ID {sellId} no encontrada");
        }
        catch (InvalidOperationException ex)
        {
            // Ej: La venta tiene movimientos asociados
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
            _logger.LogError(ex, "Error en DeleteSell");
            return StatusCode(500, $"Error interno del servidor {ex.Message}");
        }
    }

    [HttpPut("{sellId}")]
    public async Task<IActionResult> UpdateSell([FromBody] CreateSellDto sellDto, int sellId, [FromHeader(Name = "companyId")] int companyId)
    {
        try
        {
            return Ok(await _service.UpdateSellAsync(sellId, sellDto));
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
            _logger.LogError(ex, "Error en UpdateSell");
            return StatusCode(500, $"Error interno del servidor: {ex.Message}");
        }
    }
}
