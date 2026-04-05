namespace MazErpAPI.DTOs.Inventory;

public record class WarehouseDto
{
    public required int WarehouseId { get; init; }
    public required int CompanyId { get; init; }
    public required string Name { get; init; }
    public string? Description { get; init; }
}
