using MazErpAPI.Enums;

namespace MazErpAPI.DTOs.Movements;

public record class MovementDto
{
    public required int MovementId { get; init; }
    public required int UserId { get; init; }
    
    public string? Description { get; init; }
    public required int Quantity { get; init; }
    public required Currency Currency { get; init; }
    public required DateTimeOffset MovementDate { get; init; }
}
