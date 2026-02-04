using MazErpBack.DTOs.Inventory;
using MazErpBack.Models;

namespace MazErpBack.Services;

public interface IProductService
{
    // Only avaible for admin pannel or backend operations
    public Task<List<ProductDto>> GetProductsAsync();
    public Task<ProductDto> GetProductByIdAsync(int productId);
    public Task<bool> DeleteProductAsync(int productId);

    // For common users
    public Task<List<ProductDto>> GetProductsByWorkflowAsync(int workflowId);
    public Task<List<ProductDto>> GetProductsByWarehouseAsync(int warehouseId);
    public Task<ProductDto> CreateProductAsync(CreateProductDto productDto);
    public Task<ProductDto> UpdateProductAsync(CreateProductDto productDto);
}