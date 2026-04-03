using MazErpBack.Enums;

namespace MazErpBack.DTOs.Movements;

public record class CreateExpenseDto
{
    public required int UserId { get; init; }
    public required int CompanyId { get; init; }
    public string? Description { get; init; }
    public ExpenseCategory ExpenseCategory { get; init; }
    public decimal Amount { get; init; }
    public PaymentMethod PaymentMethod { get; init; }
    public DateTimeOffset DatePaid { get; init; }
}
