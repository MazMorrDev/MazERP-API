
using Microsoft.AspNetCore.Mvc;

namespace MazErpBack;

[ApiController]
[Route("api/[controller]")]
public class ProductsController(ProductsService productsService) : ControllerBase
{
    private readonly ProductsService _productService = productsService;


    [HttpPost("create")]
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

    [HttpDelete("delete")]
    public async Task<IActionResult> DeleteProduct ([FromBody] DeleteProductDto productDto)
    {
        try
        {
            return Ok(await _productService.DeleteProductAsync(productDto));
        }
        catch (Exception)
        {
            throw;
        }
    }
}
