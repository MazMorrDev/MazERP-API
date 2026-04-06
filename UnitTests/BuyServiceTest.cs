using MazErpAPI.Context;
using MazErpAPI.DTOs.Movements;
using MazErpAPI.Enums;
using MazErpAPI.Models;
using MazErpAPI.Services.Implementation;
using MazErpAPI.Services.Interfaces;
using MazErpAPI.Utils;
using MazErpAPI.Utils.Mappers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace UnitTests;

public class BuyServiceTests
{
    private readonly AppDbContext _context;
    private readonly Mock<IUserService> _mockUserService;
    private readonly Mock<ISupplierService> _mockSupplierService;
    private readonly Mock<IInventoryService> _mockInventoryService;
    private readonly BuyMapper _mapper;
    private readonly BuyService _buyService;

    public BuyServiceTests()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: $"TestDb_{Guid.NewGuid()}")
            .Options;

        _context = new AppDbContext(options);
        _mockUserService = new Mock<IUserService>();
        _mockSupplierService = new Mock<ISupplierService>();
        _mockInventoryService = new Mock<IInventoryService>();
        var mockLogger = new Mock<ILogger<BuyService>>();
        var mockMapperLogger = new Mock<ILogger<BuyMapper>>();
        _mapper = new BuyMapper(mockMapperLogger.Object);
        _buyService = new BuyService(_context, _mapper, _mockUserService.Object, _mockSupplierService.Object, _mockInventoryService.Object, mockLogger.Object);
    }

    #region CreateBuyAsync Tests

    [Fact]
    public async Task CreateBuyAsync_ValidData_ReturnsBuyDto()
    {
        // Arrange
        var createBuyDto = new CreateBuyDto
        {
            SupplierId = 1,
            UserId = 1,
            InventoryId = 1,
            Currency = Currency.USD,
            Description = "Test Buy",
            MovementDate = DateTime.UtcNow,
            UnitaryCost = 100.50m,
            ReceivedQuantity = 10,
            DeliveryStatus = DeliveryStatus.Pending
        };

        var supplier = new Supplier { Id = 1, Name = "Test Supplier" };
        var user = new User { Id = 1, Email = "test@example.com", Name = "Test User" };
        var inventory = new Inventory { Id = 1, Name = "Test Inventory" };

        _mockSupplierService.Setup(x => x.GetSupplierByIdAsync(1)).ReturnsAsync(supplier);
        _mockUserService.Setup(x => x.GetUserByIdAsync(1)).ReturnsAsync(user);
        _mockInventoryService.Setup(x => x.GetInventoryByIdAsync(1)).ReturnsAsync(inventory);

        // Act
        var result = await _buyService.CreateBuyAsync(createBuyDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(createBuyDto.SupplierId, result.SupplierId);
        Assert.Equal(createBuyDto.UserId, result.UserId);
        Assert.Equal(createBuyDto.InventoryId, result.InventoryId);
        Assert.Equal(createBuyDto.Currency, result.Currency);
        Assert.Equal(createBuyDto.Description, result.Description);
        Assert.Equal(createBuyDto.UnitaryCost, result.UnitaryCost);
        Assert.Equal(createBuyDto.ReceivedQuantity, result.ReceivedQuantity);

        // Verificar que se guardaron en BD
        var savedMovement = await _context.Movements.FirstOrDefaultAsync(m => m.Description == "Test Buy");
        var savedBuy = await _context.Buys.FirstOrDefaultAsync(b => b.SupplierId == 1);

        Assert.NotNull(savedMovement);
        Assert.NotNull(savedBuy);
        Assert.True(savedMovement.IsActive);
    }

    [Fact]
    public async Task CreateBuyAsync_WhenSupplierNotFound_ThrowsException()
    {
        // Arrange
        var createBuyDto = new CreateBuyDto { SupplierId = 999, UserId = 1, InventoryId = 1 };
        _mockSupplierService.Setup(x => x.GetSupplierByIdAsync(999)).ThrowsAsync(new KeyNotFoundException());

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => _buyService.CreateBuyAsync(createBuyDto));
    }

    [Fact]
    public async Task CreateBuyAsync_WhenUserNotFound_ThrowsException()
    {
        // Arrange
        var createBuyDto = new CreateBuyDto { SupplierId = 1, UserId = 999, InventoryId = 1 };
        _mockSupplierService.Setup(x => x.GetSupplierByIdAsync(1)).ReturnsAsync(new Supplier());
        _mockUserService.Setup(x => x.GetUserByIdAsync(999)).ThrowsAsync(new KeyNotFoundException());

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => _buyService.CreateBuyAsync(createBuyDto));
    }

    [Fact]
    public async Task CreateBuyAsync_WhenInventoryNotFound_ThrowsException()
    {
        // Arrange
        var createBuyDto = new CreateBuyDto { SupplierId = 1, UserId = 1, InventoryId = 999 };
        _mockSupplierService.Setup(x => x.GetSupplierByIdAsync(1)).ReturnsAsync(new Supplier());
        _mockUserService.Setup(x => x.GetUserByIdAsync(1)).ReturnsAsync(new User());
        _mockInventoryService.Setup(x => x.GetInventoryByIdAsync(999)).ThrowsAsync(new KeyNotFoundException());

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => _buyService.CreateBuyAsync(createBuyDto));
    }

    #endregion

    #region GetBuyByIdAsync Tests

    [Fact]
    public async Task GetBuyByIdAsync_ExistingBuy_ReturnsBuy()
    {
        // Arrange
        var movement = new Movement { Id = 1, IsActive = true };
        var buy = new Buy { Id = 1, MovementId = 1, SupplierId = 1 };
        await _context.Movements.AddAsync(movement);
        await _context.Buys.AddAsync(buy);
        await _context.SaveChangesAsync();

        // Act
        var result = await _buyService.GetBuyByIdAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
    }

    [Fact]
    public async Task GetBuyByIdAsync_NonExistingBuy_ThrowsKeyNotFoundException()
    {
        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => _buyService.GetBuyByIdAsync(999));
    }

    #endregion

    #region GetBuysByInventoryAsync Tests

    [Fact]
    public async Task GetBuysByInventoryAsync_WithExistingBuys_ReturnsPaginatedResult()
    {
        // Arrange
        var inventoryId = 1;
        var movements = new List<Movement>();
        var buys = new List<Buy>();

        for (int i = 1; i <= 5; i++)
        {
            var movement = new Movement { Id = i, IsActive = true, Description = $"Buy {i}" };
            var buy = new Buy { Id = i, MovementId = i, InventoryId = inventoryId, SupplierId = i };
            movements.Add(movement);
            buys.Add(buy);
        }

        await _context.Movements.AddRangeAsync(movements);
        await _context.Buys.AddRangeAsync(buys);
        await _context.SaveChangesAsync();

        // Act
        var result = await _buyService.GetBuysByInventoryAsync(inventoryId, 1, 2);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(5, result.TotalCount);
        Assert.Equal(2, result.Items.Count());
        Assert.Equal(1, result.PageNumber);
        Assert.Equal(2, result.PageSize);
    }

    [Fact]
    public async Task GetBuysByInventoryAsync_WithNoBuys_ReturnsEmptyResult()
    {
        // Act
        var result = await _buyService.GetBuysByInventoryAsync(999, 1, 10);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(0, result.TotalCount);
        Assert.Empty(result.Items);
    }

    [Fact]
    public async Task GetBuysByInventoryAsync_IgnoresInactiveMovements()
    {
        // Arrange
        var inventoryId = 1;
        var activeMovement = new Movement { Id = 1, IsActive = true, Description = "Active Buy" };
        var inactiveMovement = new Movement { Id = 2, IsActive = false, Description = "Inactive Buy" };

        var activeBuy = new Buy { Id = 1, MovementId = 1, InventoryId = inventoryId };
        var inactiveBuy = new Buy { Id = 2, MovementId = 2, InventoryId = inventoryId };

        await _context.Movements.AddRangeAsync(activeMovement, inactiveMovement);
        await _context.Buys.AddRangeAsync(activeBuy, inactiveBuy);
        await _context.SaveChangesAsync();

        // Act
        var result = await _buyService.GetBuysByInventoryAsync(inventoryId, 1, 10);

        // Assert
        Assert.Single(result.Items);
    }

    #endregion

    #region UpdateBuyAsync Tests

    [Fact]
    public async Task UpdateBuyAsync_ValidUpdate_ReturnsUpdatedBuyDto()
    {
        // Arrange
        var movement = new Movement { Id = 1, IsActive = true, Currency = Currency.USD, Description = "Original", MovementDate = DateTime.UtcNow, UserId = 1 };
        var buy = new Buy { Id = 1, MovementId = 1, SupplierId = 1, UnitaryCost = 100, ReceivedQuantity = 5, DeliveryStatus = DeliveryStatus.Pending };

        await _context.Movements.AddAsync(movement);
        await _context.Buys.AddAsync(buy);
        await _context.SaveChangesAsync();

        var updateDto = new CreateBuyDto
        {
            SupplierId = 2,
            Currency = Currency.EUR,
            Description = "Updated",
            MovementDate = DateTime.UtcNow.AddDays(1),
            UserId = 2,
            UnitaryCost = 200,
            ReceivedQuantity = 10,
            DeliveryStatus = DeliveryStatus.Completed
        };

        // Act
        var result = await _buyService.UpdateBuyAsync(1, updateDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(updateDto.SupplierId, result.SupplierId);
        Assert.Equal(updateDto.Currency, result.Currency);
        Assert.Equal(updateDto.Description, result.Description);
        Assert.Equal(updateDto.UnitaryCost, result.UnitaryCost);
        Assert.Equal(updateDto.ReceivedQuantity, result.ReceivedQuantity);
        Assert.Equal(updateDto.DeliveryStatus, result.DeliveryStatus);
    }

    [Fact]
    public async Task UpdateBuyAsync_NonExistingBuy_ThrowsKeyNotFoundException()
    {
        // Arrange
        var updateDto = new CreateBuyDto { SupplierId = 1 };

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => _buyService.UpdateBuyAsync(999, updateDto));
    }

    #endregion

    #region DeleteBuyAsync Tests

    [Fact]
    public async Task DeleteBuyAsync_ExistingBuy_RemovesFromDatabase()
    {
        // Arrange
        var movement = new Movement { Id = 1, IsActive = true };
        var buy = new Buy { Id = 1, MovementId = 1 };

        await _context.Movements.AddAsync(movement);
        await _context.Buys.AddAsync(buy);
        await _context.SaveChangesAsync();

        // Act
        await _buyService.DeleteBuyAsync(1);

        // Assert
        Assert.Null(await _context.Buys.FindAsync(1));
        Assert.Null(await _context.Movements.FindAsync(1));
    }

    [Fact]
    public async Task DeleteBuyAsync_NonExistingBuy_ThrowsKeyNotFoundException()
    {
        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => _buyService.DeleteBuyAsync(999));
    }

    #endregion

    #region SoftDeleteBuyAsync Tests

    [Fact]
    public async Task SoftDeleteBuyAsync_ExistingBuy_ReturnsTrue()
    {
        // Arrange
        var movement = new Movement { Id = 1, IsActive = true };
        await _context.Movements.AddAsync(movement);
        await _context.SaveChangesAsync();

        // Act
        var result = await _buyService.SoftDeleteBuyAsync(1);

        // Assert
        Assert.True(result);
        var deletedMovement = await _context.Movements.FindAsync(1);
        Assert.False(deletedMovement?.IsActive);
    }

    [Fact]
    public async Task SoftDeleteBuyAsync_NonExistingBuy_ReturnsFalse()
    {
        // Act
        var result = await _buyService.SoftDeleteBuyAsync(999);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task SoftDeleteBuyAsync_AlreadyInactiveMovement_ReturnsFalse()
    {
        // Arrange
        var movement = new Movement { Id = 1, IsActive = false };
        await _context.Movements.AddAsync(movement);
        await _context.SaveChangesAsync();

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => _buyService.SoftDeleteBuyAsync(1));
    }

    #endregion
}