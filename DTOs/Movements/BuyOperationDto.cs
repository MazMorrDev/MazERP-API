using MazErpBack.Enums;

namespace MazErpBack.DTOs.Movements;

public record class BuyOperationDto : MovementOperationDto
{
    public required int SupplierId { get; init; }
    public required DeliveryStatus DeliveryStatus { get; init; }
    public required decimal UnitaryCost { get; init; }
    public required int ReceivedQuantity { get; init; }
}
