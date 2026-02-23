
using MazErpBack.Context;
using MazErpBack.DTOs.Movements;
using MazErpBack.Models;
using MazErpBack.Services.Interfaces;
using MazErpBack.Utils.Mappers;
using Microsoft.EntityFrameworkCore;

namespace MazErpBack.Services.Implementation;

public class ExpenseService(ExpenseMapper mapper, ILogger<ExpenseService> logger, AppDbContext context, IUserService userService, ICompanyService companyService) : IExpenseService
{
    private readonly ExpenseMapper _mapper = mapper;
    private readonly ILogger<ExpenseService> _logger = logger;
    private readonly IUserService _userService = userService;
    private readonly ICompanyService _companyService = companyService;
    private readonly AppDbContext _context = context;

    public async Task<ExpenseDto> CreateExpenseAsync(CreateExpenseDto expenseDto)
    {
        var user = await _userService.GetUserByIdAsync(expenseDto.UserId);
        var company = await _companyService.GetCompanyByIdAsync(expenseDto.CompanyId);
        var expense = _mapper.MapModelToDto(user, company, expenseDto);
        await _context.Expenses.AddAsync(expense);
        await _context.SaveChangesAsync();
        return _mapper.MapToDto(expense);
    }

    public async Task DeleteExpenseAsync(int expenseId)
    {
        var expense = await GetExpenseByIdAsync(expenseId);
        _context.Expenses.Remove(expense);
        await _context.SaveChangesAsync();
    }

    public async Task<Expense> GetExpenseByIdAsync(int expenseId)
    {
        return await _context.Expenses.FindAsync(expenseId) ?? throw new KeyNotFoundException($"Expense with id: {expenseId} not found");
    }

    public async Task<List<ExpenseDto>> GetExpensesByCompanyAsync(int companyId)
    {
        List<ExpenseDto> expenses = _mapper.MapListToDto(await _context.Expenses.Where(e => e.CompanyId == companyId && e.IsActive).ToListAsync());
        return expenses;
    }

    public async Task<bool> SoftDeleteExpenseAsync(int expenseId)
    {
        try
        {
            var expense = await GetExpenseByIdAsync(expenseId);
            expense.IsActive = false;
            await _context.SaveChangesAsync();
            return true;
        }
        catch
        {
            return false;
        }

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
