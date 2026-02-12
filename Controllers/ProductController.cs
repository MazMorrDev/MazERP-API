using MazErpBack.DTOs.Inventory;
using MazErpBack.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MazErpBack.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class ProductsController(IProductService productsService) : ControllerBase
{
    private readonly IProductService _productService = productsService;


    [HttpPost]
    public async Task<IActionResult> CreateProduct([FromBody] CreateProductDto productDto)
    {
        try
        {
            return Ok(await _productService.CreateProductAsync(productDto));
        }
        catch (Exception)
        {
            throw;
        }
    }
}
