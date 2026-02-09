
using MazErpBack.Context;
using MazErpBack.DTOs.Movements;
using MazErpBack.Models;
using MazErpBack.Services.Interfaces;
using MazErpBack.Utils.Mappers;
using Microsoft.EntityFrameworkCore;

namespace MazErpBack.Services.Implementation;

public class ExpenseService(ExpenseMapper mapper, ILogger<ExpenseService> logger, AppDbContext context) : IExpenseService
{
    private readonly ExpenseMapper _mapper = mapper;
    private readonly ILogger<ExpenseService> _logger = logger;
    private readonly AppDbContext _context = context;

    public async Task<ExpenseDto> CreateExpenseAsync(CreateExpenseDto expenseDto)
    {
        var user = await _context.Users.FindAsync(expenseDto.UserId);
        var company = await _context.Companies.FindAsync(expenseDto.CompanyId);
        if (user == null || company == null)
        {
            _logger.LogDebug("No existe el usuario o la compañia");
            ArgumentNullException.ThrowIfNull(user);
            ArgumentNullException.ThrowIfNull(company);
        }

        var expense = _mapper.MapModelToDto(user, company, expenseDto);
        await _context.Expenses.AddAsync(expense);
        await _context.SaveChangesAsync();
        return _mapper.MapToDto(expense);
    }

    public async Task<bool> DeleteExpenseAsync(int expenseId)
    {
        var expense = await _context.Expenses.FindAsync(expenseId);
        if (expense == null)
        {
            _logger.LogDebug("");
            return false;
        }
        _context.Expenses.Remove(expense);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<Expense> GetExpenseByIdAsync(int expenseId)
    {
        var expense = await _context.Expenses.FindAsync(expenseId);
        ArgumentNullException.ThrowIfNull(expense);
        return expense;
    }

    public async Task<List<ExpenseDto>> GetExpensesByCompanyAsync(int companyId)
    {
        List<ExpenseDto> expenses = _mapper.MapListToDto(await _context.Expenses.Where(e => e.CompanyId == companyId).ToListAsync());
        return expenses;
    }

    public async Task<bool> SoftDeleteExpenseAsync(int expenseId)
    {
        var expense = await _context.Expenses.FindAsync(expenseId);
        if (expense == null)
        {
            // Logging
            return false;
        }
        expense.IsActive = false;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<ExpenseDto> UpdateExpenseAsync(int id, CreateExpenseDto expenseDto)
    {
        var expense = GetExpenseByIdAsync(id).Result;
        expense.Amount = expenseDto.Amount;
        expense.UserId = expenseDto.UserId;
        expense.Description = expenseDto.Description;
        expense.Category = expenseDto.ExpenseCategory;
        expense.PaymentMethod = expenseDto.PaymentMethod;
        expense.DatePaid = expenseDto.DatePaid;

        await _context.SaveChangesAsync();
        return _mapper.MapToDto(expense);
    }
}
