using MazErpAPI.Context;
using MazErpAPI.DTOs.Inventory;
using MazErpAPI.Models;
using MazErpAPI.Services.Implementation;
using MazErpAPI.Services.Interfaces;
using MazErpAPI.Utils.Mappers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace UnitTests;

public class ProductServiceTest
{
    private readonly Mock<ILogger<ProductService>> _mockLogger;
    private readonly ProductMapper _mapper;
    private readonly AppDbContext _context;
    private readonly ProductService _productService;

    public ProductServiceTest()
    {
        _mockLogger = new Mock<ILogger<ProductService>>();
        _mapper = new ProductMapper();

        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDb_" + Guid.NewGuid())
            .Options;

        _context = new AppDbContext(options);
        _productService = new ProductService(_context, _mapper);
    }

    [Fact]
    public async Task GetProductByIdAsync_WithExistingId_ReturnsProduct()
    {
        // Arrange
        var product = new Product 
        { 
            Id = 1, 
            Name = "Test Product",
            IsActive = true
        };
        
        await _context.Products.AddAsync(product);
        await _context.SaveChangesAsync();

        // Act
        var result = await _productService.GetProductByIdAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(product.Id, result.Id);
        Assert.Equal(product.Name, result.Name);
    }

    [Fact]
    public async Task GetProductByIdAsync_WithNonExistingId_ThrowsKeyNotFoundException()
    {
        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => 
            _productService.GetProductByIdAsync(999));
    }

    [Fact]
    public async Task CreateProductAsync_WithValidData_ReturnsProduct()
    {
        // Arrange
        var createInventoryAndProductDto = new CreateInventoryAndProductDto
        {
            ProductName = "Test Product",
            ProductDescription = "Test Description",
            ProductPhotoUrl = "http://example.com/photo.jpg",
            ProductCategory = ProductCategory.Electronics,
            BasePrice = 100.00m,
            BaseDiscount = 10,
            AverageCost = 80.00m,
            AlertStock = 5,
            WarehouseId = 1
        };

        // Act
        var result = await _productService.CreateProductAsync(createInventoryAndProductDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(createInventoryAndProductDto.ProductName, result.Name);
        Assert.Equal(createInventoryAndProductDto.ProductDescription, result.Description);
        Assert.Equal(createInventoryAndProductDto.ProductCategory, result.Category);
        Assert.True(result.Id > 0);
        
        var savedProduct = await _context.Products.FirstOrDefaultAsync(p => p.Name == createInventoryAndProductDto.ProductName);
        Assert.NotNull(savedProduct);
    }

    [Fact]
    public async Task UpdateProductAsync_WithExistingId_ReturnsUpdatedProduct()
    {
        // Arrange
        var product = new Product 
        { 
            Id = 1, 
            Name = "Original Name",
            Description = "Original Description",
            IsActive = true
        };
        
        await _context.Products.AddAsync(product);
        await _context.SaveChangesAsync();

        var updateInventoryProductDto = new UpdateInventoryProductDto
        {
            ProductName = "Updated Name",
            ProductDescription = "Updated Description",
            ProductPhotoUrl = "http://example.com/updated.jpg",
            ProductCategory = ProductCategory.Clothing,
            BasePrice = 150.00m,
            BaseDiscount = 15,
            AverageCost = 120.00m,
            AlertStock = 10,
            WarehouseId = 1
        };

        // Act
        var result = await _productService.UpdateProductAsync(1, updateInventoryProductDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(updateInventoryProductDto.ProductName, result.Name);
        Assert.Equal(updateInventoryProductDto.ProductDescription, result.Description);
        Assert.Equal(updateInventoryProductDto.ProductCategory, result.Category);
    }

    [Fact]
    public async Task UpdateProductAsync_WithNonExistingId_ThrowsKeyNotFoundException()
    {
        // Arrange
        var updateInventoryProductDto = new UpdateInventoryProductDto
        {
            ProductName = "Updated Name",
            ProductDescription = "Updated Description",
            ProductPhotoUrl = "http://example.com/updated.jpg",
            ProductCategory = ProductCategory.Clothing,
            BasePrice = 150.00m,
            BaseDiscount = 15,
            AverageCost = 120.00m,
            AlertStock = 10,
            WarehouseId = 1
        };

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => 
            _productService.UpdateProductAsync(999, updateInventoryProductDto));
    }

    [Fact]
    public async Task DeleteProductAsync_WithExistingId_RemovesProduct()
    {
        // Arrange
        var product = new Product 
        { 
            Id = 1, 
            Name = "Test Product",
            IsActive = true
        };
        
        await _context.Products.AddAsync(product);
        await _context.SaveChangesAsync();

        // Act
        await _productService.DeleteProductAsync(1);

        // Assert
        var deletedProduct = await _context.Products.FindAsync(1);
        Assert.Null(deletedProduct);
    }

    [Fact]
    public async Task DeleteProductAsync_WithNonExistingId_DoesNotThrow()
    {
        // Act (should not throw exception even if not found)
        await _productService.DeleteProductAsync(999);

        // Assert - No assertion needed as we just verify it doesn't throw
    }
}
