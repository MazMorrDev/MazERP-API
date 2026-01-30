using MazErpBack.Models;

namespace MazErpBack.Services;

public interface IExpenseService
{
        // Only avaible for admin pannel or backend operations
    public Task<List<Expense>> GetExpensesAsync();
    public Task<Expense> GetExpenseById(int id);
    public Task<Expense> DeleteExpenseAsync(int expenseId);

    // For common users
    public Task<List<Expense>> GetExpensesByWorkflowAsync(int workflowId);
    public Task<Expense> CreateExpenseAsync(CreateExpenseDto expenseDto);
    public Task<Expense> UpdateSExpenseAsync(UpdateExpenseDto expenseDto);
    public Task<Expense> SoftDeleteExpenseAsync(int expenseId);
}
