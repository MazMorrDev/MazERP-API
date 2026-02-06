
using MazErpBack.DTOs.Movements;
using MazErpBack.Models;
using MazErpBack.Services.Interfaces;

namespace MazErpBack.Services.Implementation;

public class ExpenseService : IExpenseService
{
    public Task<ExpenseDto> CreateExpenseAsync(CreateExpenseDto expenseDto)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteExpenseAsync(int expenseId)
    {
        throw new NotImplementedException();
    }

    public Task<Expense> GetExpenseByIdAsync(int expenseId)
    {
        throw new NotImplementedException();
    }

    public Task<List<Expense>> GetExpensesAsync()
    {
        throw new NotImplementedException();
    }

    public Task<List<ExpenseDto>> GetExpensesByCompanyAsync(int companyId)
    {
        throw new NotImplementedException();
    }

    public Task<bool> SoftDeleteExpenseAsync(int expenseId)
    {
        throw new NotImplementedException();
    }

    public Task<ExpenseDto> UpdateExpenseAsync(int id, CreateExpenseDto expenseDto)
    {
        throw new NotImplementedException();
    }
}
