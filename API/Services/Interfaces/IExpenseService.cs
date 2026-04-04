using MazErpAPI.DTOs.Movements;
using MazErpAPI.Models;
using MazErpBack.Utils;

namespace MazErpAPI.Services.Interfaces;

public interface IExpenseService
{
    // Only avaible for admin pannel or backend operations
    public Task<Expense> GetExpenseByIdAsync(int expenseId);
    public Task DeleteExpenseAsync(int expenseId);

    // For common users
    public Task<PaginatedResult<ExpenseDto>> GetExpensesByCompanyAsync(int companyId, int pageNumber, int pageSize);
    public Task<ExpenseDto> CreateExpenseAsync(CreateExpenseDto expenseDto);
    public Task<ExpenseDto> UpdateExpenseAsync(int id, CreateExpenseDto expenseDto);
    public Task<bool> SoftDeleteExpenseAsync(int expenseId);
}
