namespace MazErpBack.DTOs.Inventory;

public record class SellPointDto
{
    public required int Id { get; init; }
    public required string Name { get; init; }
    public string? Description { get; init; }
    public string? Location { get; init; }
}
