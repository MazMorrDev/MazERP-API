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
    public async Task<IActionResult> GetBuysByInventory(int inventoryId)
    {
        try
        {
            return Ok(await _service.GetBuysByInventoryAsync(inventoryId));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateBuy([FromHeader(Name = "companyId")] int companyId, CreateBuyDto createBuyDto)
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
    public async Task<IActionResult> DeleteBuy([FromHeader(Name = "companyId")] int companyId, int buyId)
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
    public async Task<IActionResult> UpdateBuy([FromHeader(Name = "companyId")] int companyId, CreateBuyDto buyDto, int buyId)
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