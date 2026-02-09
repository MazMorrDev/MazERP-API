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

    public async Task<ProductDto> CreateProductAsync(CreateProductDto productDto)
    {
        try
        {
            var product = _mapper.MapDtoToModel(productDto);

            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();

            return _mapper.MapToDto(product);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public async Task<Product> GetProductByIdAsync(int productId)
    {
        var product = await _context.Products.FindAsync(productId);
        ArgumentNullException.ThrowIfNull(product);
        return product;
    }

    public async Task<List<ProductDto>> GetProductsByCompanyAsync(int companyId)
    {
        throw new NotImplementedException();
    }

    public async Task<List<ProductDto>> GetProductsByWarehouseAsync(int warehouseId)
    {
        throw new NotImplementedException();
    }

    public async Task<ProductDto> UpdateProductAsync(int productId, CreateProductDto productDto)
    {
        var product = await GetProductByIdAsync(productId);
        product.Name = productDto.Name;
        product.Description = productDto.Description;
        product.PhotoUrl = productDto.PhotoUrl;
        product.Category = productDto.ProductCategory;

        await _context.SaveChangesAsync();
        return _mapper.MapToDto(product);
    }

    public async Task<bool> DeleteProductAsync(int productId)
    {
        try
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null)
            {
                return false;
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return true;
        }
        catch (NullReferenceException)
        {
            throw;
        }
    }
}
