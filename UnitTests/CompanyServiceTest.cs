using MazErpAPI.Context;
using MazErpAPI.DTOs.Company;
using MazErpAPI.Models;
using MazErpAPI.Services.Implementation;
using MazErpAPI.Services.Interfaces;
using MazErpAPI.Utils.Mappers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace UnitTests;

public class CompanyServiceTest
{
    private readonly Mock<ILogger<CompanyService>> _mockLogger;
    private readonly Mock<IUserService> _mockUserService;
    private readonly CompanyMapper _mapper;
    private readonly AppDbContext _context;
    private readonly CompanyService _companyService;

    public CompanyServiceTest()
    {
        _mockLogger = new Mock<ILogger<CompanyService>>();
        _mockUserService = new Mock<IUserService>();
        _mapper = new CompanyMapper();

        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDb_" + Guid.NewGuid())
            .Options;

        _context = new AppDbContext(options);
        _companyService = new CompanyService(_context, _mockLogger.Object, _mapper, _mockUserService.Object);
    }

    [Fact]
    public async Task GetCompanyByIdAsync_WithExistingId_ReturnsCompany()
    {
        // Arrange
        var company = new Company 
        { 
            Id = 1, 
            Name = "Test Company",
            IsActive = true
        };
        
        await _context.Companies.AddAsync(company);
        await _context.SaveChangesAsync();

        // Act
        var result = await _companyService.GetCompanyByIdAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(company.Id, result.Id);
        Assert.Equal(company.Name, result.Name);
    }

    [Fact]
    public async Task GetCompanyByIdAsync_WithNonExistingId_ThrowsKeyNotFoundException()
    {
        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => 
            _companyService.GetCompanyByIdAsync(999));
    }

    [Fact]
    public async Task AddUserToCompanyAsync_WithValidData_ReturnsUserCompanyDto()
    {
        // Arrange
        var user = new User { Id = 1, Email = "user@example.com", IsActive = true };
        var company = new Company { Id = 1, Name = "Test Company", IsActive = true };
        
        _mockUserService.Setup(s => s.GetUserByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(user);

        var addUserToCompanyDto = new AddUserToCompanyDto
        {
            UserId = 1,
            Role = UserCompanyRole.Owner
        };

        // Act
        var result = await _companyService.AddUserToCompanyAsync(addUserToCompanyDto, 1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(addUserToCompanyDto.UserId, result.UserId);
        Assert.Equal(addUserToCompanyDto.Role, result.Role);
        
        var savedUserCompany = await _context.UserCompanies.FirstOrDefaultAsync(uc => uc.UserId == addUserToCompanyDto.UserId);
        Assert.NotNull(savedUserCompany);
    }

    [Fact]
    public async Task AddUserToCompanyAsync_WithExistingAssociation_ThrowsBadHttpRequestException()
    {
        // Arrange
        var user = new User { Id = 1, Email = "user@example.com", IsActive = true };
        var company = new Company { Id = 1, Name = "Test Company", IsActive = true };
        var existingUserCompany = new UserCompany
        {
            UserId = 1,
            CompanyId = 1,
            Role = UserCompanyRole.Owner,
            AssignedAt = DateTimeOffset.UtcNow,
            User = user,
            Company = company
        };
        
        _mockUserService.Setup(s => s.GetUserByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(user);
        
        await _context.UserCompanies.AddAsync(existingUserCompany);
        await _context.SaveChangesAsync();

        var addUserToCompanyDto = new AddUserToCompanyDto
        {
            UserId = 1,
            Role = UserCompanyRole.Admin
        };

        // Act & Assert
        await Assert.ThrowsAsync<BadHttpRequestException>(() => 
            _companyService.AddUserToCompanyAsync(addUserToCompanyDto, 1));
    }
}
