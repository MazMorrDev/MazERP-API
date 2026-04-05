using MazErpAPI.DTOs.Inventory;
using MazErpAPI.Models;

namespace MazErpAPI.Utils.Mappers;

public class InventoryMapper
{
    public InventoryDto MapToDto(Inventory inventory, Product product)
    {
        return new InventoryDto
        {
            InventoryId = inventory.Id,
            WarehouseId = inventory.WarehouseId,
            ProductId = inventory.ProductId,
            Stock = inventory.Stock,
            BaseDiscount = inventory.BaseDiscount,
            BasePrice = inventory.BasePrice,
            AverageCost = inventory.AverageCost,
            AlertStock = inventory.AlertStock,
            WarningStock = inventory.WarningStock,
            ProductName = product.Name,
            ProductCategory = product.Category,
            ProductDescription = product.Description,
            PhotoUrl = product.PhotoUrl
        };
    }

    public List<InventoryDto> MapListToDto(List<Inventory> inventories, List<Product> products)
    {
        List<InventoryDto> inventoriesDto = [];
        foreach (var item in inventories)
        {
            var product = products.FirstOrDefault(p => p.Id == item.ProductId) ?? throw new KeyNotFoundException("Product compatible no encontrado");
            inventoriesDto.Add(MapToDto(item, product));
        }
        return inventoriesDto;
    }

    public Inventory MapDtoToModel(Warehouse warehouse, Product product, CreateInventoryAndProductDto inventoryDto)
    {
        return new Inventory
        {
            WarehouseId = inventoryDto.WarehouseId,
            ProductId = product.Id,
            Stock = inventoryDto.Stock,
            BasePrice = inventoryDto.BasePrice,
            BaseDiscount = inventoryDto.BaseDiscount,
            AverageCost = inventoryDto.AverageCost,
            AlertStock = inventoryDto.AlertStock,
            WarningStock = inventoryDto.WarningStock,
            Warehouse = warehouse,
            Product = product
        };
    }

    public Inventory MapDtoByProductToModel(Warehouse warehouse, Product product, CreateInventoryByExistentProductDto inventoryDto)
    {
        return new Inventory
        {
            WarehouseId = inventoryDto.WarehouseId,
            ProductId = product.Id,
            Stock = inventoryDto.Stock,
            BasePrice = inventoryDto.BasePrice,
            BaseDiscount = inventoryDto.BaseDiscount,
            AverageCost = inventoryDto.AverageCost,
            AlertStock = inventoryDto.AlertStock,
            WarningStock = inventoryDto.WarningStock,
            Warehouse = warehouse,
            Product = product
        };
    }
}
