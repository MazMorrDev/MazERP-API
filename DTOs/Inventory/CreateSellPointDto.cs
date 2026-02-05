namespace MazErpBack.DTOs.Inventory;

public record class CreateSellPointDto
{
    public required string Name { get; set; }
    public string? Location { get; set; }
}
