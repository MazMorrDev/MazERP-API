using MazErpAPI.Context;
using MazErpAPI.DTOs.Inventory;
using MazErpAPI.Models;
using MazErpAPI.Services.Interfaces;
using MazErpBack.Utils.Mappers;

namespace MazErpBack.Services.Implementation;

public class ProductService(AppDbContext context, ProductMapper mapper) : IProductService
{
    private readonly AppDbContext _context = context;
    private readonly ProductMapper _mapper = mapper;

    public async Task<Product> CreateProductAsync(CreateInventoryAndProductDto productDto)
    {
        try
        {
            var product = _mapper.MapDtoToModel(productDto);

            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();

            return product;
        }
        catch (Exception)
        {

            throw;
        }
    }

    public async Task<Product> GetProductByIdAsync(int productId)
    {
        var product = await _context.Products.FindAsync(productId) ?? throw new KeyNotFoundException($"Product with id: {productId} not found");
        return product;
    }

    public async Task<Product> UpdateProductAsync(int productId, UpdateInventoryProductDto productDto)
    {
        var product = await GetProductByIdAsync(productId);
        product.Name = productDto.ProductName;
        product.Description = productDto.ProductDescription;
        product.PhotoUrl = productDto.PhotoUrl;
        product.Category = productDto.ProductCategory;

        await _context.SaveChangesAsync();
        return product;
    }

    public async Task DeleteProductAsync(int productId)
    {
        try
        {
            var product = await GetProductByIdAsync(productId);
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
        }
        catch (NullReferenceException)
        {
            throw;
        }
    }
}
