
using MazErpAPI.Context;
using MazErpAPI.DTOs.Movements;
using MazErpAPI.Models;
using MazErpAPI.Services.Interfaces;
using MazErpAPI.Utils;
using MazErpAPI.Utils.Mappers;
using Microsoft.EntityFrameworkCore;

namespace MazErpAPI.Services.Implementation;

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

    public async Task<PaginatedResult<ExpenseDto>> GetExpensesByCompanyAsync(int companyId, int pageNumber, int pageSize)
    {
        var query = _context.Expenses.Where(e => e.CompanyId == companyId && e.IsActive);
        var totalCount = await query.CountAsync();
        var items = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
        var expensesDto = _mapper.MapListToDto(items);
        return new PaginatedResult<ExpenseDto>(expensesDto, totalCount, pageNumber, pageSize);
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
