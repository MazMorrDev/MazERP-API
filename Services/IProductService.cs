using MazErpBack.DTOs.Products;
using MazErpBack.Models;

namespace MazErpBack.Services;

public interface IProductService
{
    // Only avaible for admin pannel or backend operations
    public Task<List<Product>> GetProductsAsync();
    public Task<Product> GetProductByIdAsync(int productId);
    public Task DeleteProductAsync(int buyId);

    // For common users
    public Task<List<Product>> GetProductsByWorkflowAsync(int workflowId);
    public Task<List<Product>> GetProductsByWarehouseAsync(int warehouseId);
    public Task<Product> CreateProductAsync(CreateProductDto productDto);
    public Task<Product> UpdateProductAsync(UpdateProductDto productDto);
    public Task<Product> SoftDeleteProductAsync(int productID);
}