using MazErpBack.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MazErpBack.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class ProductController(IProductService productService) : ControllerBase
{
    private readonly IProductService _productService = productService;

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetProductById(int id)
    {
        try
        {
            var product = await _productService.GetProductByIdAsync(id);
            return Ok(product);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound($"Producto no encontrado: {ex.Message}");
        }
        catch (UnauthorizedAccessException ex)
        {
            return Forbid($"Permiso denegado: {ex.Message}");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error interno del servidor: {ex.Message}");
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        try
        {
            await _productService.DeleteProductAsync(id);
            return NoContent(); // 204 - Eliminación exitosa
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound($"Producto con ID {id} no encontrado: {ex.Message}");
        }
        catch (UnauthorizedAccessException ex)
        {
            return Forbid($"Permiso denegado: {ex.Message}");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error interno del servidor: {ex.Message}");
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateProduct([FromBody] MazErpBack.DTOs.Inventory.CreateInventoryAndProductDto productDto)
    {
        try
        {
            var product = await _productService.CreateProductAsync(productDto);
            return CreatedAtAction(nameof(GetProductById), new { id = product.Id }, product);
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
            return StatusCode(500, $"Error interno del servidor: {ex.Message}");
        }
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateProduct(int id, [FromBody] MazErpBack.DTOs.Inventory.UpdateInventoryProductDto productDto)
    {
        try
        {
            var product = await _productService.UpdateProductAsync(id, productDto);
            return Ok(product);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound($"Producto con ID {id} no encontrado: {ex.Message}");
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
            return StatusCode(500, $"Error interno del servidor: {ex.Message}");
        }
    }
}
