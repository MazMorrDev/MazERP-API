using MazErpAPI.DTOs.Company;
using MazErpAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Npgsql;

namespace MazErpAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class CompanyController(ICompanyService service, ILogger<CompanyController> logger) : ControllerBase
{
    private readonly ICompanyService _service = service;
    private readonly ILogger<CompanyController> _logger = logger;

    [HttpGet("by-user/{userId}")]
    public async Task<IActionResult> GetCompaniesByUser(int userId, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        try
        {
            // Validaciones
            if (pageNumber < 1 || pageSize < 1 || pageSize > 100)
                return BadRequest("Parámetros de paginación inválidos");

            var result = await _service.GetCompaniesByUser(userId, pageNumber, pageSize);
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
            _logger.LogError(ex, "Error en GetCompaniesByUser");
            return StatusCode(500, $"Error interno del servidor: {ex.Message}");
        }
    }

    [HttpPost("assign")]
    public async Task<IActionResult> AddUserToCompany([FromBody] AddUserToCompanyDto dto, [FromHeader(Name = "companyId")] int companyId)
    {
        try
        {
            return Ok(await _service.AddUserToCompanyAsync(dto, companyId));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound($"Recurso no encontrado: {ex.Message}");
        }
        catch (BadHttpRequestException ex)
        {
            return BadRequest($"Solicitud inválida: {ex.Message}");
        }
        catch (UnauthorizedAccessException ex)
        {
            return Forbid($"Permiso denegado: {ex.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error en AddUserToCompany");
            return StatusCode(500, $"Error interno del servidor: {ex.Message}");
        }
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateCompany([FromBody] CreateCompanyDto Company)
    {
        try
        {
            return Ok(await _service.CreateCompanyAsync(Company));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound($"Recurso no encontrado: {ex.Message}");
        }
        catch (NpgsqlException ex)
        {
            _logger.LogError(ex, "Error de base de datos en CreateCompany");
            return StatusCode(500, $"Error interno del servidor: {ex.Message}");
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
            _logger.LogError(ex, "Error en CreateCompany");
            return StatusCode(500, $"Error interno del servidor: {ex.Message}");
        }
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteCompany([FromHeader(Name = "companyId")] int companyId)
    {
        try
        {
            var deleted = await _service.SoftDeleteCompanyAsync(companyId);

            if (deleted)
                return NoContent(); // 204 - Eliminación exitosa
            else
                return NotFound($"Empresa con ID {companyId} no encontrada");
        }
        catch (InvalidOperationException ex)
        {
            // Ej: La empresa tiene usuarios asociados
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
            _logger.LogError(ex, "Error en DeleteCompany");
            return StatusCode(500, $"Error interno del servidor {ex.Message}");
        }
    }
}
