using MazErpBack.DTOs.Products;
using MazErpBack.Models;

namespace MazErpBack.Services;

public interface IProductService
{
    public Task<Product> CreateProductAsync(CreateProductDto productDto);

    public Task DeleteProductAsync(int id);
}