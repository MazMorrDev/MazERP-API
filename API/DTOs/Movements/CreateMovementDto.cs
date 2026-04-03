using MazErpBack.Enums;

namespace MazErpBack.DTOs.Movements;

public record class CreateMovementDto
{
    public required int UserId { get; init; }
    public string? Description { get; init; }
    public required int Quantity { get; init; }
    public required Currency Currency { get; init; }
    public required DateTimeOffset MovementDate { get; init; }
}
