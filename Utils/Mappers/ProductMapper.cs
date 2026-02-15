using MazErpBack.DTOs.Inventory;
using MazErpBack.Models;

namespace MazErpBack.Utils.Mappers;

public class ProductMapper
{
    public Product MapDtoToModel(CreateInventoryAndProductDto productDto)
    {
        return new Product{
            Name = productDto.ProductName,
            Description = productDto.ProductDescription,
            PhotoUrl = productDto.PhotoUrl,
            Category = productDto.ProductCategory
        };
    }
}
