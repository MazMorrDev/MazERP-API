using MazErpBack.Context;
using MazErpBack.DTOs.Inventory;
using MazErpBack.Models;
using MazErpBack.Services.Interfaces;
using MazErpBack.Utils.Mappers;
using Microsoft.EntityFrameworkCore;

namespace MazErpBack.Services.Implementation;

public class ProductService(AppDbContext context, ProductMapper mapper) : IProductService
{
    private readonly AppDbContext _context = context;
    private readonly ProductMapper _mapper = mapper;

    public async Task<Product> CreateProductAsync(CreateProductDto productDto)
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
        ArgumentNullException.ThrowIfNull(product);
        return product;
    }

    public async Task<List<Product>> GetProductsByCompanyAsync(int companyId)
    {
        var warehouses = _context.Warehouses.Where(w => w.CompanyId == companyId);
        List<Product> products = [];
        foreach (var w in warehouses)
        {
            products.AddRange(await GetProductsByWarehouseAsync(w.Id));
        }
        return products;
    }

    public async Task<List<Product>> GetProductsByWarehouseAsync(int warehouseId)
    {
        var inventories = await _context.Inventories.Where(i => i.WarehouseId == warehouseId).ToListAsync();
        List<Product> products = [];
        foreach (var inventory in inventories)
        {
            products.AddRange(await _context.Products.Where(p => p.Id == inventory.Id).ToListAsync());
        }
        return products;
    }

    public async Task<Product> UpdateProductAsync(int productId, CreateProductDto productDto)
    {
        var product = await GetProductByIdAsync(productId);
        product.Name = productDto.Name;
        product.Description = productDto.Description;
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
