
using Microsoft.AspNetCore.Mvc;

namespace MazErpBack;

[ApiController]
[Route("api/[controller]")]
public class ProductsController(ProductsService productsService) : ControllerBase
{
    private readonly ProductsService _productService = productsService;


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

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        try
        {
            return Ok(await _productService.DeleteProductAsync(id));
        }
        catch (Exception)
        {
            throw;
        }
    }
}
