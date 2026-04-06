using MazErpAPI.Context;
using MazErpAPI.DTOs.Movements;
using MazErpAPI.Enums;
using MazErpAPI.Models;
using MazErpAPI.Services.Interfaces;
using MazErpAPI.Utils;
using MazErpAPI.Utils.Mappers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace UnitTests.Services;

public class SellServiceTests
{
    private readonly AppDbContext _context;
    private readonly Mock<ISellPointService> _mockSellPointService;
    private readonly Mock<IUserService> _mockUserService;
    private readonly SellMapper _mapper;
    private readonly SellService _sellService;

    public SellServiceTests()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: $"TestDb_{Guid.NewGuid()}")
            .Options;

        _context = new AppDbContext(options);
        _mockSellPointService = new Mock<ISellPointService>();
        _mockUserService = new Mock<IUserService>();
        var mockLogger = new Mock<ILogger<SellService>>();
        var mockMapperLogger = new Mock<ILogger<SellMapper>>();
        _mapper = new SellMapper(mockMapperLogger.Object);
        _sellService = new SellService(_context, _mapper, _mockSellPointService.Object, _mockUserService.Object, mockLogger.Object);
    }

    #region CreateSellAsync Tests

    [Fact]
    public async Task CreateSellAsync_ValidData_ReturnsSellDto()
    {
        // Arrange
        var createSellDto = new CreateSellDto
        {
            SellPointId = 1,
            UserId = 1,
            Currency = Currency.USD,
            Description = "Test Sell",
            MovementDate = DateTime.UtcNow,
            DiscountPercentage = 10.5m,
            PaymentStatus = PaymentStatus.Pending,
            SaleType = SaleType.Retail,
            SellerNotes = "Test notes"
        };

        var sellPoint = new SellPoint { Id = 1, Name = "Test SellPoint" };
        var user = new User { Id = 1, Email = "test@example.com", Name = "Test User" };

        _mockSellPointService.Setup(x => x.GetSellPointByIdAsync(1)).ReturnsAsync(sellPoint);
        _mockUserService.Setup(x => x.GetUserByIdAsync(1)).ReturnsAsync(user);

        // Act
        var result = await _sellService.CreateSellAsync(createSellDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(createSellDto.SellPointId, result.SellPointId);
        Assert.Equal(createSellDto.UserId, result.UserId);
        Assert.Equal(createSellDto.Currency, result.Currency);
        Assert.Equal(createSellDto.Description, result.Description);
        Assert.Equal(createSellDto.DiscountPercentage, result.DiscountPercentage);
        Assert.Equal(createSellDto.PaymentStatus, result.PaymentStatus);
        Assert.Equal(createSellDto.SaleType, result.SaleType);
        Assert.Equal(createSellDto.SellerNotes, result.SellerNotes);

        // Verificar que se guardaron en BD
        var savedMovement = await _context.Movements.FirstOrDefaultAsync(m => m.Description == "Test Sell");
        var savedSell = await _context.Sells.FirstOrDefaultAsync(s => s.SellPointId == 1);

        Assert.NotNull(savedMovement);
        Assert.NotNull(savedSell);
        Assert.True(savedMovement.IsActive);
    }

    [Fact]
    public async Task CreateSellAsync_WhenSellPointNotFound_ThrowsException()
    {
        // Arrange
        var createSellDto = new CreateSellDto { SellPointId = 999, UserId = 1 };
        _mockSellPointService.Setup(x => x.GetSellPointByIdAsync(999)).ThrowsAsync(new KeyNotFoundException());

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => _sellService.CreateSellAsync(createSellDto));
    }

    [Fact]
    public async Task CreateSellAsync_WhenUserNotFound_ThrowsException()
    {
        // Arrange
        var createSellDto = new CreateSellDto { SellPointId = 1, UserId = 999 };
        _mockSellPointService.Setup(x => x.GetSellPointByIdAsync(1)).ReturnsAsync(new SellPoint());
        _mockUserService.Setup(x => x.GetUserByIdAsync(999)).ThrowsAsync(new KeyNotFoundException());

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => _sellService.CreateSellAsync(createSellDto));
    }

    #endregion

    #region GetSellByIdAsync Tests

    [Fact]
    public async Task GetSellByIdAsync_ExistingSell_ReturnsSell()
    {
        // Arrange
        var movement = new Movement { Id = 1, IsActive = true };
        var sell = new Sell { Id = 1, MovementId = 1, SellPointId = 1 };
        await _context.Movements.AddAsync(movement);
        await _context.Sells.AddAsync(sell);
        await _context.SaveChangesAsync();

        // Act
        var result = await _sellService.GetSellByIdAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
    }

    [Fact]
    public async Task GetSellByIdAsync_NonExistingSell_ThrowsKeyNotFoundException()
    {
        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => _sellService.GetSellByIdAsync(999));
    }

    [Fact]
    public async Task GetSellByIdAsync_WithInactiveMovement_ThrowsKeyNotFoundException()
    {
        // Arrange
        var movement = new Movement { Id = 1, IsActive = false };
        var sell = new Sell { Id = 1, MovementId = 1, SellPointId = 1 };
        await _context.Movements.AddAsync(movement);
        await _context.Sells.AddAsync(sell);
        await _context.SaveChangesAsync();

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => _sellService.GetSellByIdAsync(1));
    }

    #endregion

    #region GetSellsBySellPointAsync Tests

    [Fact]
    public async Task GetSellsBySellPointAsync_WithExistingSells_ReturnsPaginatedResult()
    {
        // Arrange
        var sellPointId = 1;
        var movements = new List<Movement>();
        var sells = new List<Sell>();

        for (int i = 1; i <= 5; i++)
        {
            var movement = new Movement { Id = i, IsActive = true, Description = $"Sell {i}" };
            var sell = new Sell { Id = i, MovementId = i, SellPointId = sellPointId };
            movements.Add(movement);
            sells.Add(sell);
        }

        await _context.Movements.AddRangeAsync(movements);
        await _context.Sells.AddRangeAsync(sells);
        await _context.SaveChangesAsync();

        // Act
        var result = await _sellService.GetSellsBySellPointAsync(sellPointId, 1, 2);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(5, result.TotalCount);
        Assert.Equal(2, result.Items.Count());
        Assert.Equal(1, result.PageNumber);
        Assert.Equal(2, result.PageSize);
    }

    [Fact]
    public async Task GetSellsBySellPointAsync_WithNoSells_ReturnsEmptyResult()
    {
        // Act
        var result = await _sellService.GetSellsBySellPointAsync(999, 1, 10);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(0, result.TotalCount);
        Assert.Empty(result.Items);
    }

    [Fact]
    public async Task GetSellsBySellPointAsync_IgnoresInactiveMovements()
    {
        // Arrange
        var sellPointId = 1;
        var activeMovement = new Movement { Id = 1, IsActive = true, Description = "Active Sell" };
        var inactiveMovement = new Movement { Id = 2, IsActive = false, Description = "Inactive Sell" };

        var activeSell = new Sell { Id = 1, MovementId = 1, SellPointId = sellPointId };
        var inactiveSell = new Sell { Id = 2, MovementId = 2, SellPointId = sellPointId };

        await _context.Movements.AddRangeAsync(activeMovement, inactiveMovement);
        await _context.Sells.AddRangeAsync(activeSell, inactiveSell);
        await _context.SaveChangesAsync();

        // Act
        var result = await _sellService.GetSellsBySellPointAsync(sellPointId, 1, 10);

        // Assert
        Assert.Single(result.Items);
    }

    #endregion

    #region UpdateSellAsync Tests

    [Fact]
    public async Task UpdateSellAsync_ValidUpdate_ReturnsUpdatedSellDto()
    {
        // Arrange
        var movement = new Movement
        {
            Id = 1,
            IsActive = true,
            Currency = Currency.USD,
            Description = "Original",
            MovementDate = DateTime.UtcNow,
            UserId = 1
        };

        var sell = new Sell
        {
            Id = 1,
            MovementId = 1,
            SellPointId = 1,
            DiscountPercentage = 5,
            PaymentStatus = PaymentStatus.Pending,
            SaleType = SaleType.Retail,
            SellerNotes = "Original notes"
        };

        await _context.Movements.AddAsync(movement);
        await _context.Sells.AddAsync(sell);
        await _context.SaveChangesAsync();

        var updateDto = new CreateSellDto
        {
            SellPointId = 2,
            Currency = Currency.EUR,
            Description = "Updated",
            MovementDate = DateTime.UtcNow.AddDays(1),
            UserId = 2,
            DiscountPercentage = 15,
            PaymentStatus = PaymentStatus.Completed,
            SaleType = SaleType.Wholesale,
            SellerNotes = "Updated notes"
        };

        // Act
        var result = await _sellService.UpdateSellAsync(1, updateDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(updateDto.SellPointId, result.SellPointId);
        Assert.Equal(updateDto.Currency, result.Currency);
        Assert.Equal(updateDto.Description, result.Description);
        Assert.Equal(updateDto.DiscountPercentage, result.DiscountPercentage);
        Assert.Equal(updateDto.PaymentStatus, result.PaymentStatus);
        Assert.Equal(updateDto.SaleType, result.SaleType);
        Assert.Equal(updateDto.SellerNotes, result.SellerNotes);
    }

    [Fact]
    public async Task UpdateSellAsync_NonExistingSell_ThrowsKeyNotFoundException()
    {
        // Arrange
        var updateDto = new CreateSellDto { SellPointId = 1 };

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => _sellService.UpdateSellAsync(999, updateDto));
    }

    [Fact]
    public async Task UpdateSellAsync_WithInactiveMovement_ThrowsKeyNotFoundException()
    {
        // Arrange
        var movement = new Movement { Id = 1, IsActive = false };
        var sell = new Sell { Id = 1, MovementId = 1, SellPointId = 1 };

        await _context.Movements.AddAsync(movement);
        await _context.Sells.AddAsync(sell);
        await _context.SaveChangesAsync();

        var updateDto = new CreateSellDto { SellPointId = 2 };

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => _sellService.UpdateSellAsync(1, updateDto));
    }

    #endregion

    #region DeleteSellAsync Tests

    [Fact]
    public async Task DeleteSellAsync_ExistingSell_RemovesFromDatabase()
    {
        // Arrange
        var movement = new Movement { Id = 1, IsActive = true };
        var sell = new Sell { Id = 1, MovementId = 1 };

        await _context.Movements.AddAsync(movement);
        await _context.Sells.AddAsync(sell);
        await _context.SaveChangesAsync();

        // Act
        await _sellService.DeleteSellAsync(1);

        // Assert
        Assert.Null(await _context.Sells.FindAsync(1));
        Assert.Null(await _context.Movements.FindAsync(1));
    }

    [Fact]
    public async Task DeleteSellAsync_NonExistingSell_ThrowsKeyNotFoundException()
    {
        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => _sellService.DeleteSellAsync(999));
    }

    #endregion

    #region SoftDeleteSellAsync Tests

    [Fact]
    public async Task SoftDeleteSellAsync_ExistingSell_ReturnsTrue()
    {
        // Arrange
        var movement = new Movement { Id = 1, IsActive = true };
        await _context.Movements.AddAsync(movement);
        await _context.SaveChangesAsync();

        // Act
        var result = await _sellService.SoftDeleteSellAsync(1);

        // Assert
        Assert.True(result);
        var deletedMovement = await _context.Movements.FindAsync(1);
        Assert.False(deletedMovement?.IsActive);
    }

    [Fact]
    public async Task SoftDeleteSellAsync_NonExistingSell_ReturnsFalse()
    {
        // Act
        var result = await _sellService.SoftDeleteSellAsync(999);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task SoftDeleteSellAsync_AlreadyInactiveMovement_ThrowsKeyNotFoundException()
    {
        // Arrange
        var movement = new Movement { Id = 1, IsActive = false };
        await _context.Movements.AddAsync(movement);
        await _context.SaveChangesAsync();

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => _sellService.SoftDeleteSellAsync(1));
    }

    #endregion
}