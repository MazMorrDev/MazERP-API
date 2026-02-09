using MazErpBack.DTOs.Movements;
using MazErpBack.Models;

namespace MazErpBack.Services.Interfaces;

public interface IExpenseService
{
    // Only avaible for admin pannel or backend operations
    public Task<Expense> GetExpenseByIdAsync(int expenseId);
    public Task DeleteExpenseAsync(int expenseId);

    // For common users
    public Task<List<ExpenseDto>> GetExpensesByCompanyAsync(int companyId);
    public Task<ExpenseDto> CreateExpenseAsync(CreateExpenseDto expenseDto);
    public Task<ExpenseDto> UpdateExpenseAsync(int id, CreateExpenseDto expenseDto);
    public Task SoftDeleteExpenseAsync(int expenseId);
}
