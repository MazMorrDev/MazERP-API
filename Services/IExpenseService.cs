using MazErpBack.DTOs.Movements;

namespace MazErpBack.Services;

public interface IExpenseService
{
    // Only avaible for admin pannel or backend operations
    public Task<List<ExpenseDto>> GetExpensesAsync();
    public Task<ExpenseDto> GetExpenseByIdAsync(int expenseId);
    public Task<bool> DeleteExpenseAsync(int expenseId);

    // For common users
    public Task<List<ExpenseDto>> GetExpensesByWorkflowAsync(int workflowId);
    public Task<ExpenseDto> CreateExpenseAsync(CreateExpenseDto expenseDto);
    public Task<ExpenseDto> UpdateExpenseAsync(CreateExpenseDto expenseDto);
    public Task<bool> SoftDeleteExpenseAsync(int expenseId);
}
