namespace MazErpBack;

public interface IProductsService
{
    public Task<Product> CreateProductAsync(CreateProductDto productDto);

    public Task<Product> DeleteProductAsync(int id);
}