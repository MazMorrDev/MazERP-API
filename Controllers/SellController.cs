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
    public async Task<IActionResult> GetBuysBySellPoint(
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
        catch (Exception ex)
        {
            // Logear el error aquí
            return StatusCode(500, ex.Message);
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateSell([FromBody] CreateSellDto createSellDto, [FromHeader(Name = "companyId")] int companyId)
    {
        try
        {
            return Ok(await _service.CreateSellAsync(createSellDto));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpDelete("{sellId}")]
    public async Task<IActionResult> DeleteSell(int sellId, [FromHeader(Name = "companyId")] int companyId)
    {
        try
        {
            return Ok(await _service.SoftDeleteSellAsync(sellId));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpPut("{sellId}")]
    public async Task<IActionResult> UpdateSell([FromBody] CreateSellDto sellDto, int sellId, [FromHeader(Name = "companyId")] int companyId)
    {
        try
        {
            return Ok(await _service.UpdateSellAsync(sellId, sellDto));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
}
