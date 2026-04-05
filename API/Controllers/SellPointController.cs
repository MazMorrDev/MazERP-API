using MazErpAPI.DTOs.Inventory;
using MazErpAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MazErpAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class SellPointController(ISellPointService service, ILogger<SellPointController> logger) : ControllerBase
{
    public ISellPointService _service = service;
    public ILogger<SellPointController> _logger = logger;

    [HttpGet]
    public async Task<IActionResult> GetSellPointsByCompany(
        [FromHeader(Name = "companyId")] int companyId,
        [FromQuery] int pageNumber = 1, 
        [FromQuery] int pageSize = 10
    )
    {
        try
        {
            // Validaciones
            if (pageNumber < 1 || pageSize < 1 || pageSize > 100)
                return BadRequest("Parámetros de paginación inválidos");

            var result = await _service.GetSellPointsByCompanyAsync(companyId, pageNumber, pageSize);
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
            _logger.LogError(ex, "Error en GetSellPointsByCompany");
            return StatusCode(500, $"Error interno del servidor: {ex.Message}");
        }
    }

    [HttpGet("{sellPointId}")]
    public async Task<IActionResult> GetSellPointById(
        int sellPointId, 
        [FromHeader(Name = "companyId")] int companyId
    )
    {
        try
        {
            var result = await _service.GetSellPointByIdAsync(sellPointId);
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
            _logger.LogError(ex, "Error en GetSellPointById");
            return StatusCode(500, $"Error interno del servidor: {ex.Message}");
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateSellPoint(
        [FromBody] CreateSellPointDto createSellPointDto, 
        [FromHeader(Name = "companyId")] int companyId
    )
    {
        try
        {
            return Ok(await _service.CreateSellPointAsync(createSellPointDto));
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
            _logger.LogError(ex, "Error en CreateSellPoint");
            return StatusCode(500, $"Error interno del servidor: {ex.Message}");
        }
    }

    [HttpPut("{sellPointId}")]
    public async Task<IActionResult> UpdateSellPoint(
        [FromBody] CreateSellPointDto sellPointDto, 
        int sellPointId, 
        [FromHeader(Name = "companyId")] int companyId
    )
    {
        try
        {
            return Ok(await _service.UpdateSellPointAsync(sellPointId, sellPointDto));
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
            _logger.LogError(ex, "Error en UpdateSellPoint");
            return StatusCode(500, $"Error interno del servidor: {ex.Message}");
        }
    }

    [HttpDelete("{sellPointId}")]
    public async Task<IActionResult> DeleteSellPoint(
        int sellPointId, 
        [FromHeader(Name = "companyId")] int companyId
    )
    {
        try
        {
            var deleted = await _service.SoftDeleteSellPointAsync(sellPointId);

            if (deleted)
                return NoContent(); // 204 - Eliminación exitosa
            else
                return NotFound($"Punto de venta con ID {sellPointId} no encontrado");
        }
        catch (InvalidOperationException ex)
        {
            // Ej: El punto de venta tiene movimientos asociados
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
            _logger.LogError(ex, "Error en DeleteSellPoint");
            return StatusCode(500, $"Error interno del servidor {ex.Message}");
        }
    }
}