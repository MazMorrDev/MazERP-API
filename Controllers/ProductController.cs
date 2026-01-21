using MazErpBack.Dtos.Products;
using MazErpBack.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MazErpBack.Controllers;

[ApiController]
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
