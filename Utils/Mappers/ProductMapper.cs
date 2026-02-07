using MazErpBack.DTOs.Inventory;
using MazErpBack.Models;

namespace MazErpBack.Utils.Mappers;

public class ProductMapper
{
    public ProductDto MapToDto(Product product)
    {
        return new ProductDto
        {
            ProductId = product.Id,
            Name = product.Name,
            Description = product.Description,
            PhotoUrl = product.PhotoUrl,
            ProductCategory = product.Category
        };
    }

    public List<ProductDto> MapListToDto(List<Product> products)
    {
        List<ProductDto> productsDto = [];
        foreach (var product in products)
        {
            productsDto.Add(MapToDto(product));
        }
        return productsDto;
    }

    public Product MapDtoToModel(CreateProductDto productDto)
    {
        return new Product{
            Name = productDto.Name,
            Description = productDto.Description,
            PhotoUrl = productDto.PhotoUrl,
            Category = productDto.ProductCategory
        };
    }
}
