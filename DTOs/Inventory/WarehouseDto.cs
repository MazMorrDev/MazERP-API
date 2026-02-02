namespace MazErpBack.DTOs.Inventory;

public record class WarehouseDto
{
    public int WarehouseId { get; init; }
    public int WorkflowId { get; init; }
    public required string Name { get; init; }
    public string? Description { get; init; }
}
