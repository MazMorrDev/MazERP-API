using MazErpBack.DTOs.Movements;
using MazErpBack.Models;

namespace MazErpBack.Services;

public interface IExpenseService
{
    // Only avaible for admin pannel or backend operations
    public Task<List<Expense>> GetExpensesAsync();
    public Task<Expense> GetExpenseByIdAsync(int expenseId);
    public Task DeleteExpenseAsync(int expenseId);

    // For common users
    public Task<List<Expense>> GetExpensesByWorkflowAsync(int workflowId);
    public Task<Expense> CreateExpenseAsync(CreateExpenseDto expenseDto);
    public Task<Expense> UpdateExpenseAsync(UpdateExpenseDto expenseDto);
    public Task<Expense> SoftDeleteExpenseAsync(int expenseId);
}
