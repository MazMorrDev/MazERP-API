using MazErpAPI.DTOs.Movements;
using MazErpAPI.Models;

namespace MazErpAPI.Utils.Mappers;

public class ExpenseMapper
{
    public ExpenseDto MapToDto(Expense expense)
    {
        return new ExpenseDto
        {
            ExpenseId = expense.Id,
            UserId = expense.UserId,
            CompanyId = expense.CompanyId,
            Description = expense.Description,
            ExpenseCategory = expense.Category,
            Amount = expense.Amount,
            PaymentMethod = expense.PaymentMethod,
            DatePaid = expense.DatePaid
        };
    }

    public Expense MapModelToDto(User user, Company company, CreateExpenseDto expenseDto)
    {
        return new Expense
        {
            UserId = expenseDto.UserId,
            CompanyId = expenseDto.CompanyId,
            Description = expenseDto.Description,
            Category = expenseDto.ExpenseCategory,
            Amount = expenseDto.Amount,
            PaymentMethod = expenseDto.PaymentMethod,
            DatePaid = expenseDto.DatePaid,
            User = user,
            Company = company
        };
    }

    public List<ExpenseDto> MapListToDto(List<Expense> expenses)
    {
        List<ExpenseDto> expensesDto = [];
        foreach (var expense in expenses)
        {
            expensesDto.Add(MapToDto(expense));
        }
        return expensesDto;
    }
}
