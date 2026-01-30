using System.ComponentModel.DataAnnotations;

namespace MazErpBack.Dtos.Products;

public record class CreateProductDto
{
    [Required(ErrorMessage = "The product name is required")]
    [MaxLength(40)]
    public string Name { get; init; } = string.Empty;

    public decimal? SellPrice { get; init; }
}
