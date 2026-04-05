using MazErpAPI.DTOs.Inventory;
using MazErpAPI.Models;

namespace MazErpAPI.Utils.Mappers;

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
