namespace MazErpBack;

public interface IProductsService
{
    public Task<Product> CreateProductAsync(CreateProductDto productDto);

    public Task<bool> DeleteProductAsync(DeleteProductDto productDto);
}