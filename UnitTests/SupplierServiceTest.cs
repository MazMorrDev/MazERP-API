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

public class SupplierServiceTest
{
    private readonly Mock<ILogger<SupplierService>> _mockLogger;
    private readonly SupplierMapper _mapper;
    private readonly AppDbContext _context;
    private readonly SupplierService _supplierService;

    public SupplierServiceTest()
    {
        _mockLogger = new Mock<ILogger<SupplierService>>();
        _mapper = new SupplierMapper();

        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDb_" + Guid.NewGuid())
            .Options;

        _context = new AppDbContext(options);
        _supplierService = new SupplierService(_context, _mapper, _mockLogger.Object);
    }

    [Fact]
    public async Task GetSupplierByIdAsync_WithExistingId_ReturnsSupplier()
    {
        // Arrange
        var supplier = new Supplier 
        { 
            Id = 1, 
            Name = "Test Supplier",
            IsActive = true
        };
        
        await _context.Suppliers.AddAsync(supplier);
        await _context.SaveChangesAsync();

        // Act
        var result = await _supplierService.GetSupplierByIdAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(supplier.Id, result.Id);
        Assert.Equal(supplier.Name, result.Name);
    }

    [Fact]
    public async Task GetSupplierByIdAsync_WithNonExistingId_ThrowsKeyNotFoundException()
    {
        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => 
            _supplierService.GetSupplierByIdAsync(999));
    }

    [Fact]
    public async Task CreateSupplierAsync_WithValidData_ReturnsSupplierDto()
    {
        // Arrange
        var createSupplierDto = new CreateSupplierDto
        {
            Name = "Test Supplier",
            ContactName = "John Doe",
            ContactPhone = "+1234567890",
            ContactEmail = "supplier@example.com",
            Address = "123 Supplier Street"
        };

        // Act
        var result = await _supplierService.CreateSupplierAsync(createSupplierDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(createSupplierDto.Name, result.Name);
        Assert.Equal(createSupplierDto.ContactName, result.ContactName);
        Assert.Equal(createSupplierDto.ContactPhone, result.ContactPhone);
        Assert.Equal(createSupplierDto.ContactEmail, result.ContactEmail);
        Assert.Equal(createSupplierDto.Address, result.Address);
        Assert.True(result.Id > 0);
        
        var savedSupplier = await _context.Suppliers.FirstOrDefaultAsync(s => s.Name == createSupplierDto.Name);
        Assert.NotNull(savedSupplier);
    }

    [Fact]
    public async Task DeleteSupplierAsync_WithExistingId_RemovesSupplier()
    {
        // Arrange
        var supplier = new Supplier 
        { 
            Id = 1, 
            Name = "Test Supplier",
            IsActive = true
        };
        
        await _context.Suppliers.AddAsync(supplier);
        await _context.SaveChangesAsync();

        // Act
        await _supplierService.DeleteSupplierAsync(1);

        // Assert
        var deletedSupplier = await _context.Suppliers.FindAsync(1);
        Assert.Null(deletedSupplier);
    }

    [Fact]
    public async Task DeleteSupplierAsync_WithNonExistingId_DoesNotThrow()
    {
        // Act (should not throw exception even if not found)
        await _supplierService.DeleteSupplierAsync(999);

        // Assert - No assertion needed as we just verify it doesn't throw
    }
}
