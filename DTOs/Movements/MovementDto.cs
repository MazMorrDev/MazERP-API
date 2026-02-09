using MazErpBack.Enums;

namespace MazErpBack.DTOs.Movements;

public record class MovementDto
{
    public required int MovementId { get; init; }
    public required int UserId { get; init; }
    public required int SellPointId { get; init; }
    public string? Description { get; init; }
    public required int Quantity { get; init; }
    public required Currency Currency { get; init; }
    public required DateTimeOffset MovementDate { get; init; }
}
