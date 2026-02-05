
using MazErpBack.DTOs.Movements;
using MazErpBack.Models;
using MazErpBack.Services.Interfaces;

namespace MazErpBack.Services.Implementation;

public class ExpenseService : IExpenseService
{
    public Task<Expense> CreateExpenseAsync(CreateExpenseDto expenseDto)
    {
        throw new NotImplementedException();
    }

    public Task<Expense> DeleteExpenseAsync(int expenseId)
    {
        throw new NotImplementedException();
    }

    public Task<Expense> GetExpenseById(int expenseId)
    {
        throw new NotImplementedException();
    }

    public Task<List<Expense>> GetExpensesAsync()
    {
        throw new NotImplementedException();
    }

    public Task<List<Expense>> GetExpensesByWorkflowAsync(int workflowId)
    {
        throw new NotImplementedException();
    }

    public Task<Expense> SoftDeleteExpenseAsync(int expenseId)
    {
        throw new NotImplementedException();
    }

    public Task<Expense> UpdateExpenseAsync(CreateExpenseDto expenseDto)
    {
        throw new NotImplementedException();
    }
}
