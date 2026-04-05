using MazErpAPI.DTOs.Inventory;
using MazErpAPI.Models;

namespace MazErpAPI.Services.Interfaces;

public interface IProductService
{
    // All Product Operations are managed by inventory Service
    public Task<Product> GetProductByIdAsync(int productId);
    public Task DeleteProductAsync(int productId);
    public Task<Product> CreateProductAsync(CreateInventoryAndProductDto productDto);
    public Task<Product> UpdateProductAsync(int productId, UpdateInventoryProductDto productDto);
}