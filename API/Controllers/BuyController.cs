using MazErpBack.DTOs.Movements;
using MazErpBack.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MazErpBack.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class BuyController(IBuyService service, ILogger<BuyController> logger) : ControllerBase
{
    public IBuyService _service = service;
    public ILogger<BuyController> _logger = logger;

    [HttpGet("by-inventory/{inventoryId}")]
    public async Task<IActionResult> GetBuysByInventory(
        int inventoryId, [FromHeader(Name = "companyId")] int companyId,
        [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10
    )
    {
        try
        {
            // Validaciones
            if (pageNumber < 1 || pageSize < 1 || pageSize > 100)
                return BadRequest("Parámetros de paginación inválidos");

            var result = await _service.GetBuysByInventoryAsync(inventoryId, pageNumber, pageSize);
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
            _logger.LogError(ex, "Error en GetBuysByInventory");
            return StatusCode(500, $"Error interno del servidor: {ex.Message}");
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateBuy([FromBody] CreateBuyDto createBuyDto, [FromHeader(Name = "companyId")] int companyId)
    {
        try
        {
            return Ok(await _service.CreateBuyAsync(createBuyDto));
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
            _logger.LogError(ex, "Error en CreateBuy");
            return StatusCode(500, $"Error interno del servidor: {ex.Message}");
        }
    }

    [HttpDelete("{buyId}")]
    public async Task<IActionResult> DeleteBuy(int buyId, [FromHeader(Name = "companyId")] int companyId)
    {
        try
        {
            var deleted = await _service.SoftDeleteBuyAsync(buyId);

            if (deleted)
                return NoContent(); // 204 - Eliminación exitosa
            else
                return NotFound($"Compra con ID {buyId} no encontrada");
        }
        catch (InvalidOperationException ex)
        {
            // Ej: La compra tiene movimientos asociados
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
            _logger.LogError(ex, "Error en DeleteBuy");
            return StatusCode(500, $"Error interno del servidor {ex.Message}");
        }
    }

    [HttpPut("{buyId}")]
    public async Task<IActionResult> UpdateBuy([FromBody] CreateBuyDto buyDto, int buyId, [FromHeader(Name = "companyId")] int companyId)
    {
        try
        {
            return Ok(await _service.UpdateBuyAsync(buyId, buyDto));
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
            _logger.LogError(ex, "Error en UpdateBuy");
            return StatusCode(500, $"Error interno del servidor: {ex.Message}");
        }
    }
}
