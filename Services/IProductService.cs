using MazErpBack.Dtos.Products;
using MazErpBack.Models;

namespace MazErpBack.Services;

public interface IProductService
{
    public Task<Product> CreateProductAsync(CreateProductDto productDto);

    public Task<Product> DeleteProductAsync(int id);
}