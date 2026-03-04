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
}