using MazErpBack.Enums;

namespace MazErpBack.DTOs.Movements;

public record class CreateDevolutionDto
{
    public int SellId { get; init; }
    public required string Reason { get; init; }
    public int RefundAmount { get; init; }
    public string? Notes { get; init; }
    public DevolutionStatus DevolutionStatus { get; init; }
    public DevolutionActionTake DevolutionActionTake { get; init; }
    public DateTimeOffset Date { get; init; }
}
