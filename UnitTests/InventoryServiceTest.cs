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

public class InventoryServiceTest
{
    private readonly Mock<ILogger<InventoryService>> _mockLogger;
    private readonly Mock<IProductService> _mockProductService;
    private readonly Mock<IWarehouseService> _mockWarehouseService;
    private readonly InventoryMapper _mapper;
    private readonly AppDbContext _context;
    private readonly InventoryService _inventoryService;

    public InventoryServiceTest()
    {
        _mockLogger = new Mock<ILogger<InventoryService>>();
        _mockProductService = new Mock<IProductService>();
        _mockWarehouseService = new Mock<IWarehouseService>();
        _mapper = new InventoryMapper();

        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDb_" + Guid.NewGuid())
            .Options;

        _context = new AppDbContext(options);
        _inventoryService = new InventoryService(_context, _mapper, _mockProductService.Object, _mockWarehouseService.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task GetInventoryByIdAsync_WithExistingId_ReturnsInventory()
    {
        // Arrange
        var inventory = new Inventory 
        { 
            Id = 1, 
            Stock = 100,
            IsActive = true
        };
        
        await _context.Inventories.AddAsync(inventory);
        await _context.SaveChangesAsync();

        // Act
        var result = await _inventoryService.GetInventoryByIdAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(inventory.Id, result.Id);
        Assert.Equal(inventory.Stock, result.Stock);
    }

    [Fact]
    public async Task GetInventoryByIdAsync_WithNonExistingId_ThrowsKeyNotFoundException()
    {
        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => 
            _inventoryService.GetInventoryByIdAsync(999));
    }

    [Fact]
    public async Task SoftDeleteInventoryAsync_WithExistingId_ReturnsTrue()
    {
        // Arrange
        var inventory = new Inventory 
        { 
            Id = 1, 
            Stock = 100,
            IsActive = true
        };
        
        await _context.Inventories.AddAsync(inventory);
        await _context.SaveChangesAsync();

        // Act
        var result = await _inventoryService.SoftDeleteInventoryAsync(1);

        // Assert
        Assert.True(result);
        
        var deletedInventory = await _context.Inventories.FindAsync(1);
        Assert.NotNull(deletedInventory);
        Assert.False(deletedInventory.IsActive);
    }

    [Fact]
    public async Task SoftDeleteInventoryAsync_WithNonExistingId_ReturnsFalse()
    {
        // Act
        var result = await _inventoryService.SoftDeleteInventoryAsync(999);

        // Assert
        Assert.False(result);
    }
}
