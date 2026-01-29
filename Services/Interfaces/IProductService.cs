using MazErpBack.Dtos.Products;
using MazErpBack.Models;

namespace MazErpBack.Services.Interfaces;

public interface IProductService
{
    public Task<Product> CreateProductAsync(CreateProductDto productDto);

    public Task<Product> DeleteProductAsync(int id);
}