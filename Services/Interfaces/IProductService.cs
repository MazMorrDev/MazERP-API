using MazErpBack.Dtos.Products;

namespace MazErpBack.Services.Interfaces;

public interface IProductService
{
    public Task<Product> CreateProductAsync(CreateProductDto productDto);

    public Task<Product> DeleteProductAsync(int id);
}