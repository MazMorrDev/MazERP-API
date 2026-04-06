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

public class SellPointServiceTest
{
    private readonly Mock<ILogger<SellPointService>> _mockLogger;
    private readonly Mock<IInventoryService> _mockInventoryService;
    private readonly SellPointMapper _mapper;
    private readonly AppDbContext _context;
    private readonly SellPointService _sellPointService;

    public SellPointServiceTest()
    {
        _mockLogger = new Mock<ILogger<SellPointService>>();
        _mockInventoryService = new Mock<IInventoryService>();
        _mapper = new SellPointMapper();

        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDb_" + Guid.NewGuid())
            .Options;

        _context = new AppDbContext(options);
        _sellPointService = new SellPointService(_context, _mapper, _mockInventoryService.Object);
    }

    [Fact]
    public async Task CreateSellPointAsync_WithValidData_ReturnsSellPointDto()
    {
        // Arrange
        var createSellPointDto = new CreateSellPointDto
        {
            Name = "Test Sell Point",
            Address = "123 Test Street",
            CompanyId = 1
        };

        // Act
        var result = await _sellPointService.CreateSellPointAsync(createSellPointDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(createSellPointDto.Name, result.Name);
        Assert.Equal(createSellPointDto.Address, result.Address);
        Assert.True(result.Id > 0);
        
        var savedSellPoint = await _context.SellPoints.FirstOrDefaultAsync(sp => sp.Name == createSellPointDto.Name);
        Assert.NotNull(savedSellPoint);
    }

    [Fact]
    public async Task GetSellPointByIdAsync_WithExistingId_ReturnsSellPoint()
    {
        // Arrange
        var sellPoint = new SellPoint 
        { 
            Id = 1, 
            Name = "Test Sell Point",
            IsActive = true
        };
        
        await _context.SellPoints.AddAsync(sellPoint);
        await _context.SaveChangesAsync();

        // Act
        var result = await _sellPointService.GetSellPointByIdAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(sellPoint.Id, result.Id);
        Assert.Equal(sellPoint.Name, result.Name);
    }

    [Fact]
    public async Task GetSellPointByIdAsync_WithNonExistingId_ThrowsKeyNotFoundException()
    {
        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => 
            _sellPointService.GetSellPointByIdAsync(999));
    }

    [Fact]
    public async Task DeleteSellPointAsync_WithExistingId_RemovesSellPoint()
    {
        // Arrange
        var sellPoint = new SellPoint 
        { 
            Id = 1, 
            Name = "Test Sell Point",
            IsActive = true
        };
        
        await _context.SellPoints.AddAsync(sellPoint);
        await _context.SaveChangesAsync();

        // Act
        await _sellPointService.DeleteSellPointAsync(1);

        // Assert
        var deletedSellPoint = await _context.SellPoints.FindAsync(1);
        Assert.Null(deletedSellPoint);
    }

    [Fact]
    public async Task DeleteSellPointAsync_WithNonExistingId_ThrowsKeyNotFoundException()
    {
        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => 
            _sellPointService.DeleteSellPointAsync(999));
    }
}
