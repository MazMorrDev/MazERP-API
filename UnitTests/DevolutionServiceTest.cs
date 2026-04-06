using MazErpAPI.Context;
using MazErpAPI.DTOs.Movements;
using MazErpAPI.Models;
using MazErpAPI.Services.Implementation;
using MazErpAPI.Services.Interfaces;
using MazErpAPI.Utils.Mappers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace UnitTests;

public class DevolutionServiceTest
{
    private readonly Mock<ILogger<DevolutionService>> _mockLogger;
    private readonly Mock<ISellService> _mockSellService;
    private readonly DevolutionMapper _mapper;
    private readonly AppDbContext _context;
    private readonly DevolutionService _devolutionService;

    public DevolutionServiceTest()
    {
        _mockLogger = new Mock<ILogger<DevolutionService>>();
        _mockSellService = new Mock<ISellService>();
        _mapper = new DevolutionMapper();

        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDb_" + Guid.NewGuid())
            .Options;

        _context = new AppDbContext(options);
        _devolutionService = new DevolutionService(_context, _mapper, _mockLogger.Object, _mockSellService.Object);
    }

    [Fact]
    public async Task CreateDevolutionAsync_WithValidData_ReturnsDevolutionDto()
    {
        // Arrange
        var sell = new Sell { Id = 1, IsActive = true };
        
        _mockSellService.Setup(s => s.GetSellByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(sell);

        var createDevolutionDto = new CreateDevolutionDto
        {
            SellId = 1,
            Amount = 50.00m,
            DevolutionActionTake = DevolutionActionTake.Restock,
            DevolutionStatus = DevolutionStatus.Pending,
            Reason = "Customer returned item"
        };

        // Act
        var result = await _devolutionService.CreateDevolutionAsync(createDevolutionDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(createDevolutionDto.Amount, result.Amount);
        Assert.Equal(createDevolutionDto.DevolutionActionTake, result.DevolutionActionTake);
        Assert.Equal(createDevolutionDto.DevolutionStatus, result.DevolutionStatus);
        Assert.Equal(createDevolutionDto.Reason, result.Reason);
        Assert.True(result.Id > 0);
        
        var savedDevolution = await _context.Devolutions.FirstOrDefaultAsync(d => d.Id == result.Id);
        Assert.NotNull(savedDevolution);
    }

    [Fact]
    public async Task GetDevolutionByIdAsync_WithExistingId_ReturnsDevolution()
    {
        // Arrange
        var devolution = new Devolution 
        { 
            Id = 1, 
            Amount = 30.00m,
            IsActive = true
        };
        
        await _context.Devolutions.AddAsync(devolution);
        await _context.SaveChangesAsync();

        // Act
        var result = await _devolutionService.GetDevolutionByIdAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(devolution.Id, result.Id);
        Assert.Equal(devolution.Amount, result.Amount);
    }

    [Fact]
    public async Task GetDevolutionByIdAsync_WithNonExistingId_ThrowsKeyNotFoundException()
    {
        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => 
            _devolutionService.GetDevolutionByIdAsync(999));
    }

    [Fact]
    public async Task DeleteDevolutionAsync_WithExistingId_RemovesDevolution()
    {
        // Arrange
        var devolution = new Devolution 
        { 
            Id = 1, 
            Amount = 30.00m,
            IsActive = true
        };
        
        await _context.Devolutions.AddAsync(devolution);
        await _context.SaveChangesAsync();

        // Act
        await _devolutionService.DeleteDevolutionAsync(1);

        // Assert
        var deletedDevolution = await _context.Devolutions.FindAsync(1);
        Assert.Null(deletedDevolution);
    }

    [Fact]
    public async Task DeleteDevolutionAsync_WithNonExistingId_DoesNotThrow()
    {
        // Act (should not throw exception even if not found)
        await _devolutionService.DeleteDevolutionAsync(999);

        // Assert - No assertion needed as we just verify it doesn't throw
    }

    [Fact]
    public async Task SoftDeleteDevolutionAsync_WithExistingId_ReturnsTrue()
    {
        // Arrange
        var devolution = new Devolution 
        { 
            Id = 1, 
            Amount = 30.00m,
            IsActive = true
        };
        
        await _context.Devolutions.AddAsync(devolution);
        await _context.SaveChangesAsync();

        // Act
        var result = await _devolutionService.SoftDeleteDevolutionAsync(1);

        // Assert
        Assert.True(result);
        
        var deletedDevolution = await _context.Devolutions.FindAsync(1);
        Assert.NotNull(deletedDevolution);
        Assert.False(deletedDevolution.IsActive);
    }

    [Fact]
    public async Task SoftDeleteDevolutionAsync_WithNonExistingId_ReturnsFalse()
    {
        // Act
        var result = await _devolutionService.SoftDeleteDevolutionAsync(999);

        // Assert
        Assert.False(result);
    }
}