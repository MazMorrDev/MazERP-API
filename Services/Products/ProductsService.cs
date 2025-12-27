namespace MazErpBack;

public class ProductsService(AppDbContext context) : IProductsService
{
    private readonly AppDbContext _context = context;

    public async Task<Product> CreateProductAsync(CreateProductDto productDto)
    {
        try
        {
            var product = new Product()
            {
                Name = productDto.Name,
                SellPrice = productDto.SellPrice
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return product;
        }
        catch (Exception)
        {

            throw;
        }
    }

    public async Task<Product> DeleteProductAsync(int id)
    {
        try
        {
            var product = await _context.Products.FindAsync(id);
            ArgumentNullException.ThrowIfNull(product);

            product.IsActive = false;
            await _context.SaveChangesAsync();

            return product;
        }
        catch (NullReferenceException)
        {
            throw;
        }
    }
}
