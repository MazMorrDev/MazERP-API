using MazErpAPI.Context;
using MazErpAPI.Enums;
using MazErpAPI.Models;
using MazErpAPI.Services.Implementation;
using MazErpAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace UnitTests;

public class RoleAuthorizationServiceTest
{
    private readonly Mock<ILogger<RoleAuthorizationService>> _mockLogger;
    private readonly AppDbContext _context;
    private readonly RoleAuthorizationService _roleAuthorizationService;

    public RoleAuthorizationServiceTest()
    {
        _mockLogger = new Mock<ILogger<RoleAuthorizationService>>();

        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDb_" + Guid.NewGuid())
            .Options;

        _context = new AppDbContext(options);
        _roleAuthorizationService = new RoleAuthorizationService(_context, _mockLogger.Object);
    }

    [Fact]
    public async Task UserHasAccessAsync_OwnerRole_ReturnsTrueForAnyZone()
    {
        // Arrange
        var userCompany = new UserCompany
        {
            UserId = 1,
            CompanyId = 1,
            Role = UserCompanyRole.Owner,
            IsActive = true
        };
        
        await _context.UserCompanies.AddAsync(userCompany);
        await _context.SaveChangesAsync();

        // Act
        var result = await _roleAuthorizationService.UserHasAccessAsync(1, 1, "/api/some/random/zone");

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task UserHasAccessAsync_AdminRole_ReturnsTrueForInventoryAndCompanyPaths()
    {
        // Arrange
        var userCompany = new UserCompany
        {
            UserId = 1,
            CompanyId = 1,
            Role = UserCompanyRole.Admin,
            IsActive = true
        };
        
        await _context.UserCompanies.AddAsync(userCompany);
        await _context.SaveChangesAsync();

        // Act
        var inventoryResult = await _roleAuthorizationService.UserHasAccessAsync(1, 1, "/api/inventory/items");
        var companyResult = await _roleAuthorizationService.UserHasAccessAsync(1, 1, "/api/company/settings");
        var otherResult = await _roleAuthorizationService.UserHasAccessAsync(1, 1, "/api/something/else");

        // Assert
        Assert.True(inventoryResult); // Should have access to inventory
        Assert.True(companyResult);   // Should have access to company
        Assert.False(otherResult);    // Should NOT have access to other paths
    }

    [Fact]
    public async Task UserHasAccessAsync_NonActiveUserCompany_ReturnsFalse()
    {
        // Arrange
        var userCompany = new UserCompany
        {
            UserId = 1,
            CompanyId = 1,
            Role = UserCompanyRole.Owner,
            IsActive = false // Not active
        };
        
        await _context.UserCompanies.AddAsync(userCompany);
        await _context.SaveChangesAsync();

        // Act
        var result = await _roleAuthorizationService.UserHasAccessAsync(1, 1, "/api/inventory");

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task GetUserRoleInCompanyAsync_ExistingUserCompany_ReturnsRole()
    {
        // Arrange
        var userCompany = new UserCompany
        {
            UserId = 1,
            CompanyId = 1,
            Role = UserCompanyRole.Owner,
            IsActive = true
        };
        
        await _context.UserCompanies.AddAsync(userCompany);
        await _context.SaveChangesAsync();

        // Act
        var result = await _roleAuthorizationService.GetUserRoleInCompanyAsync(1, 1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(UserCompanyRole.Owner, result);
    }

    [Fact]
    public async Task GetUserRoleInCompanyAsync_NonExistingUserCompany_ReturnsNull()
    {
        // Act
        var result = await _roleAuthorizationService.GetUserRoleInCompanyAsync(999, 999);

        // Assert
        Assert.Null(result);
    }
}
