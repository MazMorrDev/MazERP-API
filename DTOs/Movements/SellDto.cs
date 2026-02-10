using MazErpBack.Enums;

namespace MazErpBack.DTOs.Movements;

public record class SellDto : MovementDto
{
    public required int SellPointId { get; init; }
    public decimal DiscountPercentage { get; init; }
    public PaymentStatus PaymentStatus { get; init; }
    public SaleType SaleType { get; init; }
    public string? SellerNotes { get; init; }
}
