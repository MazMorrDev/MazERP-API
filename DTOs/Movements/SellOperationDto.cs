using MazErpBack.Enums;

namespace MazErpBack.DTOs.Movements;

public record class SellOperationDto : MovementOperationDto
{
    public decimal DiscountPercentage { get; init; }
    public PaymentStatus PaymentStatus { get; init; }
    public SaleType SaleType { get; init; }
    public string? SellerNotes { get; init; }
}
