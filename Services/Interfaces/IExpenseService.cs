using MazErpBack.DTOs.Movements;
using MazErpBack.Models;

namespace MazErpBack.Services.Interfaces;

public interface IExpenseService
{
    // Only avaible for admin pannel or backend operations
    public Task<List<Expense>> GetExpensesAsync();
    public Task<Expense> GetExpenseByIdAsync(int expenseId);
    public Task<bool> DeleteExpenseAsync(int expenseId);

    // For common users
    public Task<List<ExpenseDto>> GetExpensesByCompanyAsync(int companyId);
    public Task<ExpenseDto> CreateExpenseAsync(CreateExpenseDto expenseDto);
    public Task<ExpenseDto> UpdateExpenseAsync(CreateExpenseDto expenseDto);
    public Task<bool> SoftDeleteExpenseAsync(int expenseId);
}
