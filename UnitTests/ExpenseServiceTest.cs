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

public class ExpenseServiceTest
{
    private readonly Mock<ILogger<ExpenseService>> _mockLogger;
    private readonly Mock<IUserService> _mockUserService;
    private readonly Mock<ICompanyService> _mockCompanyService;
    private readonly ExpenseMapper _mapper;
    private readonly AppDbContext _context;
    private readonly ExpenseService _expenseService;

    public ExpenseServiceTest()
    {
        _mockLogger = new Mock<ILogger<ExpenseService>>();
        _mockUserService = new Mock<IUserService>();
        _mockCompanyService = new Mock<ICompanyService>();
        _mapper = new ExpenseMapper();

        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDb_" + Guid.NewGuid())
            .Options;

        _context = new AppDbContext(options);
        _expenseService = new ExpenseService(_mapper, _mockLogger.Object, _context, _mockUserService.Object, _mockCompanyService.Object);
    }

    [Fact]
    public async Task CreateExpenseAsync_WithValidData_ReturnsExpenseDto()
    {
        // Arrange
        var user = new User { Id = 1, Email = "user@example.com", IsActive = true };
        var company = new Company { Id = 1, Name = "Test Company", IsActive = true };
        
        _mockUserService.Setup(s => s.GetUserByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(user);
        _mockCompanyService.Setup(s => s.GetCompanyByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(company);

        var createExpenseDto = new CreateExpenseDto
        {
            UserId = 1,
            CompanyId = 1,
            Description = "Test expense",
            ExpenseCategory = ExpenseCategory.Office,
            Amount = 100.50m,
            PaymentMethod = PaymentMethod.Cash,
            DatePaid = DateTimeOffset.Now
        };

        // Act
        var result = await _expenseService.CreateExpenseAsync(createExpenseDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(createExpenseDto.Description, result.Description);
        Assert.Equal(createExpenseDto.Amount, result.Amount);
        Assert.True(result.Id > 0);
        
        var savedExpense = await _context.Expenses.FirstOrDefaultAsync(e => e.Description == createExpenseDto.Description);
        Assert.NotNull(savedExpense);
    }

    [Fact]
    public async Task GetExpenseByIdAsync_WithExistingId_ReturnsExpense()
    {
        // Arrange
        var expense = new Expense 
        { 
            Id = 1, 
            Description = "Test expense",
            Amount = 50.00m,
            IsActive = true
        };
        
        await _context.Expenses.AddAsync(expense);
        await _context.SaveChangesAsync();

        // Act
        var result = await _expenseService.GetExpenseByIdAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expense.Id, result.Id);
        Assert.Equal(expense.Description, result.Description);
    }

    [Fact]
    public async Task GetExpenseByIdAsync_WithNonExistingId_ThrowsKeyNotFoundException()
    {
        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => 
            _expenseService.GetExpenseByIdAsync(999));
    }

    [Fact]
    public async Task DeleteExpenseAsync_WithExistingId_RemovesExpense()
    {
        // Arrange
        var expense = new Expense 
        { 
            Id = 1, 
            Description = "Test expense",
            Amount = 50.00m,
            IsActive = true
        };
        
        await _context.Expenses.AddAsync(expense);
        await _context.SaveChangesAsync();

        // Act
        await _expenseService.DeleteExpenseAsync(1);

        // Assert
        var deletedExpense = await _context.Expenses.FindAsync(1);
        Assert.Null(deletedExpense);
    }

    [Fact]
    public async Task DeleteExpenseAsync_WithNonExistingId_ThrowsKeyNotFoundException()
    {
        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => 
            _expenseService.DeleteExpenseAsync(999));
    }
}
