using MazErpBack.DTOs.Inventory;
using MazErpBack.Models;

namespace MazErpBack.Services.Interfaces;

public interface IProductService
{
    // Only avaible for admin pannel or backend operations
    public Task<Product> GetProductByIdAsync(int productId);
    public Task DeleteProductAsync(int productId);

    // For common users
    public Task<List<ProductDto>> GetProductsByCompanyAsync(int companyId);
    public Task<List<ProductDto>> GetProductsByWarehouseAsync(int warehouseId);
    public Task<ProductDto> CreateProductAsync(CreateProductDto productDto);
    public Task<ProductDto> UpdateProductAsync(int productId, CreateProductDto productDto);
}