using MazErpBack.Context;
using MazErpBack.DTOs.Products;
using MazErpBack.Models;

namespace MazErpBack.Services.Implementation;

public class ProductService(AppDbContext context) : IProductService
{
    private readonly AppDbContext _context = context;

    public async Task<Product> CreateProductAsync(CreateProductDto productDto)
    {
        try
        {
            var product = new Product()
            {
                Name = productDto.Name
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

            // product.IsActive = false;
            await _context.SaveChangesAsync();

            return product;
        }
        catch (NullReferenceException)
        {
            throw;
        }
    }
}
