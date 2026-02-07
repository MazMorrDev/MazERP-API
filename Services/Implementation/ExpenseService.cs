
using MazErpBack.Context;
using MazErpBack.DTOs.Movements;
using MazErpBack.Models;
using MazErpBack.Services.Interfaces;
using MazErpBack.Utils.Mappers;

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
            _logger.LogDebug("No existe el suuario o la compañia brindados");
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
        throw new NotImplementedException();
    }

    public async Task<List<Expense>> GetExpensesAsync()
    {
        throw new NotImplementedException();
    }

    public async Task<List<ExpenseDto>> GetExpensesByCompanyAsync(int companyId)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> SoftDeleteExpenseAsync(int expenseId)
    {
        throw new NotImplementedException();
    }

    public async Task<ExpenseDto> UpdateExpenseAsync(int id, CreateExpenseDto expenseDto)
    {
        throw new NotImplementedException();
    }
}
