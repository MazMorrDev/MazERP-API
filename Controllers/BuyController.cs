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
        catch (Exception ex)
        {
            // Logear el error aquí
            return StatusCode(500, ex.Message);
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateBuy(CreateBuyDto createBuyDto, [FromHeader(Name = "companyId")] int companyId)
    {
        try
        {
            return Ok(await _service.CreateBuyAsync(createBuyDto));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpDelete("{buyId}")]
    public async Task<IActionResult> DeleteBuy(int buyId, [FromHeader(Name = "companyId")] int companyId)
    {
        try
        {
            return Ok(await _service.SoftDeleteBuyAsync(buyId));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpPut("{buyId}")]
    public async Task<IActionResult> UpdateBuy(CreateBuyDto buyDto, int buyId, [FromHeader(Name = "companyId")] int companyId)
    {
        try
        {
            return Ok(await _service.UpdateBuyAsync(buyId, buyDto));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
}